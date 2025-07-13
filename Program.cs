using Drawing;
using Map;
using SDL2;

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
var map = new DungeonMap<Tile>(256, 256);
var viewPort = new ViewPort(640, 480);
var camera = new Camera(viewPort, map.Center, 32);

var sheet = SdlAbstractions.Texture.LoadTexture(renderer, "dungeonfont.png");
var tileAtlas = new TileAtlas(sheet, 32);
tileAtlas.AddGridTile(Tile.Wall, 32, 3, 2);
tileAtlas.AddGridTile(Tile.Floor, 32, 14, 2);
var screen = new Screen(renderer);

world.Systems.Add(new DrawMapSystem(map, camera, tileAtlas, screen));



bool PollEvents()
{
    while (SDL.SDL_PollEvent(out var e) == 1)
    {
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_QUIT:
                return false;
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