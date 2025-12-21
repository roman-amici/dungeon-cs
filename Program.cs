using Drawing;
using Ecs;
using Game;
using GameDebug;
using Input;
using Map;
using SDL2;
using SdlAbstractions;

if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
{
    throw new Exception("Failed to start SDL2");
}

var window = SDL.SDL_CreateWindow(
    "Tutorial",
    SDL.SDL_WINDOWPOS_UNDEFINED,
    SDL.SDL_WINDOWPOS_UNDEFINED,
    640,
    480,
    SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

if (window == IntPtr.Zero)
{
    var err = SDL.SDL_GetError();
    throw new Exception($"Failed to create window: {err}");
}

var renderer = SDL.SDL_CreateRenderer(
    window,
    -1,
    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

if (renderer == IntPtr.Zero)
{
    var err = SDL.SDL_GetError();
    throw new Exception($"Failed to initialize renderer: {err}");
}

if (SDL_ttf.TTF_Init() < 0)
{
    var err = SDL.SDL_GetError();
    throw new Exception($"Failed to initialize ttf: {err}");
}

var world = new World();
var mapGenerator = new MapGenerator(256, 256);
var rng = new Random();
world.AddResource(rng);

var map = mapGenerator.Generate(rng);
world.AddResource(map);

var viewPort = new ViewPort(640, 480);
world.AddResource(viewPort);

var camera = new Camera(viewPort, map.Center, 32);
world.AddResource(camera);

var sheet = Texture.LoadTexture(renderer, "dungeonfont.png");
var mapTileAtlas = new TileAtlas<MapTile>(sheet, 32);
world.AddResource(mapTileAtlas);

mapTileAtlas.AddGridTile(MapTile.Wall, 32, 3, 2);
mapTileAtlas.AddGridTile(MapTile.Floor, 32, 14, 2);

var spriteAtlas = new TileAtlas<SpriteTile>(sheet, 32);
spriteAtlas.AddGridTile(SpriteTile.Knight, 32, 0, 4);
spriteAtlas.AddGridTile(SpriteTile.Goblin, 32, 7, 6);
spriteAtlas.AddGridTile(SpriteTile.Orc, 32, 15, 6);
spriteAtlas.AddGridTile(SpriteTile.Entin, 32, 15, 4);
spriteAtlas.AddGridTile(SpriteTile.Ogre, 32, 5, 4);
spriteAtlas.AddGridTile(SpriteTile.Amulet, 32, 12, 7);
spriteAtlas.AddGridTile(SpriteTile.Sword, 32, 3, 5);
spriteAtlas.AddGridTile(SpriteTile.Potion, 32, 1, 2);
world.AddResource(spriteAtlas);

var screen = new Screen(renderer);
world.AddResource(screen);

var font = Font.LoadFont("FreeMono.ttf");
var textRenderer = new TextRenderer(screen, font);
world.AddResource(textRenderer);

world.AddResource(new Turn()
{
    CurrentTurn = GameTurn.AwaitingInput
});
world.AddResource(new GameStateResource(GameState.GameRun));

var sprites = new Table<SpriteKey<SpriteTile>>();
var positions = new Table<Position>();
var player = new Singleton<Player>();
var enemies = new Table<Enemy>();
var toolTips = new Table<ToolTip>();
var healths = new Table<Health>();
var damages = new Table<Damage>();
var randomMovers = new Table<MovingRandomly>();
var pickupItems = new Table<PickupItem>();
var colliders = new Table<Collision>();
world.AddComponent(sprites);
world.AddComponent(positions);
world.AddComponent(player);
world.AddComponent(enemies);
world.AddComponent(toolTips);
world.AddComponent(healths);
world.AddComponent(damages);
world.AddComponent(randomMovers);
world.AddComponent(pickupItems);
world.AddComponent(colliders);

var inventory = new PlayerInventory();
world.AddResource(inventory);

world.SpawnEntity(new PlayerSpawner(map, sprites,positions, player, healths, toolTips, damages, colliders), null);

var enemySpawner = new EnemySpawner(map, positions, sprites, enemies, toolTips, healths, damages, randomMovers, colliders, rng);
enemySpawner.SpawnEnemies(world);

var distanceMap = new DistanceMap(map.Width,map.Height);
world.AddResource(distanceMap);

var itemSpawner = new ItemSpawner(map, distanceMap, new SingletonJoin<Player, Position>(player,positions), positions, pickupItems, sprites);
itemSpawner.SpawnItems(world, rng);

var inputParser = new InputParser();
world.AddResource(inputParser);

world.AddResource(new Queue<WantsToMoveMessage>());
world.AddResource(new Queue<WantsToAttackMessage>());

var mouseLocation = new MouseLocation();
world.AddResource(mouseLocation);

var turnScheduler = world.CreateSystem<TurnStateScheduler>();

var playerActionQueue = new Queue<PlayerAction>();
world.AddResource(playerActionQueue);

world.AddSingletonJoin<Player,Health>();
var playerActions = world.CreateSystem<PlayerActionSystem>();

world.AddTableJoin<MovingRandomly, Position>();
var moveRandomly = world.CreateSystem<MoveRandomlySystem>();

world.AddSingletonJoin<Player,Position>();
var playerInput = world.CreateSystem<PlayerInputSystem>();

world.AddTableJoin<Collision,Position>();
var movement  = world.CreateSystem<MovementSystem>(); 
var combat = world.CreateSystem<CombatSystem>();
var kill = world.CreateSystem<KillEntitiesSystem>();

var centerCamera = world.CreateSystem<CenterCameraOnPlayerSystem>();
var drawMap = world.CreateSystem<DrawMapSystem>();

world.AddTableJoin<SpriteKey<SpriteTile>,Position>();
var drawSprite = world.CreateSystem<DrawSpriteSystem>();

world.AddTableJoin<ToolTip,Position>();
var drawTooltips = world.CreateSystem<DrawTooltipSystem>();

world.AddTableJoin<Health,Position>();
var drawHealthBar = world.CreateSystem<DrawHealthBarSystem>();

world.AddTableJoin<PickupItem,Position>();
var playerPickupItem = world.CreateSystem<PickupItemSystem>(); 
var debugDraw = world.CreateSystem<DistanceDisplaySystem>();

var endTurn = world.CreateSystem<EndTurnSystem>();

turnScheduler.AwaitingInput.AddRange([
   playerInput,
   endTurn,
   centerCamera,
   drawMap,
   drawSprite,
   drawTooltips,
   drawHealthBar,
   //debugDraw,
]);

turnScheduler.PlayerTurn.AddRange([
    playerActions,
    movement,
    playerPickupItem,
    combat,
    kill,
    endTurn,
    centerCamera,
    drawMap,
    drawSprite,
    drawTooltips,
    drawHealthBar,
]);

turnScheduler.EnemyTurn.AddRange([
    moveRandomly,
    movement,
    combat,
    kill,
    endTurn,
    centerCamera,
    drawMap,
    drawSprite,
   drawTooltips,
   drawHealthBar,
]);

var menus = world.CreateSystem<MenuScheduler>();
var overlays = world.CreateSystem<OverlaysSystem>();
overlays.Menu = menus;
overlays.Gameplay = turnScheduler;

bool PollEvents()
{
    inputParser.StartNewFrame();
    while (SDL.SDL_PollEvent(out var e) == 1)
    {
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_QUIT:
                return false;
            default:
                inputParser.HandleEvent(e);
                break;
        }
    }

    return true;
}


var running = true;
while (running)
{
    SDL.SDL_RenderClear(renderer);
    running = PollEvents();
    overlays.Execute();
    SDL.SDL_RenderPresent(renderer);
}

SDL.SDL_DestroyRenderer(renderer);
SDL.SDL_DestroyWindow(window);
SDL.SDL_Quit();

Console.WriteLine("Exiting");