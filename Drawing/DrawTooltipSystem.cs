using System.Drawing;
using Ecs;
using Game;
using Input;

namespace Drawing;

public class DrawTooltipSystem(
    Camera camera,
    TextRenderer textReader,
    MouseLocation mouseLocation,
    TableJoin<ToolTip, Position> tooltipPositions) : GameSystem
{
    public override void Execute()
    {
        if (mouseLocation.Point == null)
        {
            return;
        }

        foreach (var (tooltip, position) in tooltipPositions)
        {
            if (camera.TileIsVisible(position.Value.MapPosition))
            {
                var mouseCoord = camera.ScreenSpaceToMapCoord(mouseLocation.Point.Value);
                if (position.Value.MapPosition == mouseCoord)
                {
                    var start = camera.MapCoordToScreenSpaceTopLeft(position.Value.MapPosition);
                    start.X += camera.TileSize / 2;
                    start.Y += camera.TileSize / 2;

                    textReader.DrawTextCentered(new (tooltip.Value.Text, 14, Color.White), start);
                }

            }
        }
    }
}