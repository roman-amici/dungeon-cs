using System.Drawing;
using Ecs;
using Game;

namespace Drawing;

public class DrawHealthBarSystem(Camera camera, Screen screen, TableJoin<Health, MapPosition> query) : GameSystem
{
    public override void Execute()
    {
        foreach (var (health, position) in query)
        {
            if (!camera.TileIsVisible(position.Coord))
            {
                continue;
            }

            var screenCoord = camera.MapCoordToScreenSpaceTopLeft(position.Coord);

            var healthBarPosition = new Rect2D()
            {
                TopLeft = new(screenCoord.X, screenCoord.Y - 7),
                BottomRight = new(screenCoord.X + camera.TileSize, screenCoord.Y)
            };

            screen.DrawRect(healthBarPosition, Color.Black);

            var redBarLength = health.CurrentHealth / health.MaxHealth * camera.TileSize;
            var redBar = new Rect2D()
            {
                TopLeft = healthBarPosition.TopLeft,
                BottomRight = new(screenCoord.X + redBarLength, healthBarPosition.BottomRight.Y)
            };
            screen.DrawRect(redBar, Color.DarkRed); 
        }
    }
}