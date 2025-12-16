using Drawing;
using Ecs;
using Game;
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
    throw new Exception("Failed to create window");
}

var renderer = SDL.SDL_CreateRenderer(
    window,
    -1,
    SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
    SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

if (renderer == IntPtr.Zero)
{
    throw new Exception("Failed to initialize renderer");
}

var world = new World();
var mapGenerator = new MapGenerator(256, 256);
var map = mapGenerator.Generate(new Random());
var viewPort = new ViewPort(640, 480);
var camera = new Camera(viewPort, map.Center, 32);

var sheet = SdlAbstractions.Texture.LoadTexture(renderer, "dungeonfont.png");
var mapTileAtlas = new TileAtlas<MapTile>(sheet, 32);

mapTileAtlas.AddGridTile(MapTile.Wall, 32, 3, 2);
mapTileAtlas.AddGridTile(MapTile.Floor, 32, 14, 2);

var spriteAtlas = new TileAtlas<SpriteTile>(sheet, 32);
spriteAtlas.AddGridTile(SpriteTile.Knight, 32, 0, 4);

var screen = new Screen(renderer);

var sprites = new Table<SpriteKey<SpriteTile>>();
var positions = new Table<Position>();
var player = new Singleton<Player>();
world.AddComponent(sprites);
world.AddComponent(positions);
world.AddComponent(player);

world.SpawnEntity(new PlayerSpawner(map, sprites,positions, player));

var inputParser = new InputParser();

var moveQueue = new Queue<WantsToMoveMessage>();
world.Systems.Add(new PlayerInputSystem(inputParser, new SingletonJoin<Player, Position>(player,positions), moveQueue));
world.Systems.Add(new MovementSystem(map, moveQueue, positions));

world.Systems.Add(new CenterCameraOnPlayerSystem(camera, new SingletonJoin<Player, Position>(player,positions)));
world.Systems.Add(new DrawMapSystem(map, camera, mapTileAtlas, screen));
world.Systems.Add(new DrawSpriteSystem(map, camera, spriteAtlas, screen, new TableJoin<SpriteKey<SpriteTile>, Position>(sprites,positions)));


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
    world.Execute();
    SDL.SDL_RenderPresent(renderer);
}

SDL.SDL_DestroyRenderer(renderer);
SDL.SDL_DestroyWindow(window);
SDL.SDL_Quit();

Console.WriteLine("Exiting");