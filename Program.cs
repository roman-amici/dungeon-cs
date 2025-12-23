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
    "Rogue Game",
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

var rng = new Random();
world.AddResource(rng);

var map = MapGenerator.GenerateRandomWalkMap(64, 64, rng);
world.AddResource(map);

var viewPort = new ViewPort(640, 480);
world.AddResource(viewPort);

var camera = new Camera(viewPort, new MapCoord(map.Center.X,map.Center.Y), 32);
world.AddResource(camera);

world.AddResource(new UILayout());
world.AddResource(new Queue<InventoryChangeMessage>());

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

world.AddComponent(new Table<SpriteKey<SpriteTile>>());
world.AddComponent(new Table<MapPosition>());
world.AddComponent(new Singleton<Player>());
world.AddComponent(new Table<Enemy>());
world.AddComponent(new Table<ToolTip>());
world.AddComponent(new Table<Health>());
world.AddComponent(new Table<Damage>());
world.AddComponent(new Table<MovingRandomly>());
world.AddComponent(new Table<PickupItem>());
world.AddComponent(new Table<Collision>());
world.AddComponent(new Table<ChasingPlayer>());

var inventory = new PlayerInventory();
world.AddResource(inventory);

var playerSpawner = world.CreateInstance<PlayerSpawner>();
playerSpawner.Execute();

var enemySpawner = world.CreateInstance<EnemySpawner>();
enemySpawner.Execute();

var distanceMap = new DistanceMap(map.Width,map.Height);
world.AddResource(distanceMap);

var itemSpawner = world.CreateInstance<ItemSpawner>();
itemSpawner.Execute();

var inputParser = new InputParser();
world.AddResource(inputParser);

world.AddResource(new Queue<WantsToMoveMessage>());
world.AddResource(new Queue<WantsToAttackMessage>());

var mouseLocation = new MouseLocation();
world.AddResource(mouseLocation);

var turnScheduler = world.CreateInstance<TurnStateScheduler>();

var playerActionQueue = new Queue<PlayerAction>();
world.AddResource(playerActionQueue);

var playerActions = world.CreateInstance<PlayerActionSystem>();
var moveRandomly = world.CreateInstance<MoveRandomlySystem>();
var chasePlayer = world.CreateInstance<ChasingPlayerSystem>();
var playerInput = world.CreateInstance<PlayerInputSystem>();
var movement  = world.CreateInstance<MovementSystem>(); 
var combat = world.CreateInstance<CombatSystem>();
var kill = world.CreateInstance<KillEntitiesSystem>();
var centerCamera = world.CreateInstance<CenterCameraOnPlayerSystem>();
var drawMap = world.CreateInstance<DrawMapSystem>();
var drawSprite = world.CreateInstance<DrawSpriteSystem>();
var drawTooltips = world.CreateInstance<DrawTooltipSystem>();
var drawHealthBar = world.CreateInstance<DrawHealthBarSystem>();
var playerPickupItem = world.CreateInstance<PickupItemSystem>();
var inventoryChangeSystem = world.CreateInstance<InventoryChangedSystem>();
var drawInventory = world.CreateInstance<DrawInventorySystem>();
var uiLayoutSystem = world.CreateInstance<UILayoutSystem>();
var checkWin = world.CreateInstance<CheckWinSystem>();
var debugDraw = world.CreateInstance<DistanceDisplaySystem>();

var endTurn = world.CreateInstance<EndTurnSystem>();

turnScheduler.AwaitingInput.AddRange([
   playerInput,
   endTurn,
   centerCamera,
   drawMap,
   drawSprite,
   drawTooltips,
   drawHealthBar,
   //debugDraw,
   uiLayoutSystem,
   drawInventory
]);

turnScheduler.PlayerTurn.AddRange([
    playerActions,
    movement,
    playerPickupItem,
    inventoryChangeSystem,
    combat,
    kill,
    checkWin,
    endTurn,
    centerCamera,
    drawMap,
    drawSprite,
    drawTooltips,
    drawHealthBar,
    uiLayoutSystem,
    drawInventory
]);

turnScheduler.EnemyTurn.AddRange([
    moveRandomly,
    chasePlayer,
    movement,
    combat,
    kill,
    endTurn,
    centerCamera,
    drawMap,
    drawSprite,
    drawTooltips,
    drawHealthBar,
    uiLayoutSystem,
    drawInventory,
]);

var menus = world.CreateInstance<MenuScheduler>();
var overlays = world.CreateInstance<OverlaysSystem>();
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