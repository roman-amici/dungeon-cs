using Ecs;
using Map;

namespace Drawing;

public class DrawMapSystem(
    DungeonMap<MapTile> map,
    Camera camera,
    TileAtlas<MapTile> atlas, 
    Screen screen) : GameSystem
{
    public override void Execute()
    {
        for (var x = camera.LeftX; x <= camera.RightX; x++)
        {
            for (var y = camera.TopY; y <= camera.BottomY; y++)
            {
                var tile = map.SafeGet(x, y) ?? default;
                var screenSpace = camera.ScreenSpaceTileTopLeft(new MapCoord(x, y));
                
                atlas.DrawTile(screen, tile, screenSpace);
                
            }
        }
    }
}