using System.Drawing;
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
        screen.SetBackground(Color.Black);
        for (var x = camera.LeftX; x <= camera.RightX; x++)
        {
            for (var y = camera.TopY; y <= camera.BottomY; y++)
            {
                if (x < 0 || y < 0)
                {
                    continue;
                }
                var coord = new MapCoord((uint)Math.Floor(x), (uint)Math.Floor(y));
                var tile = map.SafeGet(coord) ?? default;
                var screenSpace = camera.MapCoordToScreenSpaceTopLeft(coord);
                
                atlas.DrawTile(screen, tile, screenSpace);
            }
        }
    }
}