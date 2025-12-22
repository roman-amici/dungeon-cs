using System.Drawing;
using Drawing;
using Ecs;
using Map;

namespace GameDebug;

public class DistanceDisplaySystem(Camera camera, TextRenderer textRenderer, DistanceMap distances) : GameSystem
{
    public override void Execute()
    {
        for (var x = 0; x < distances.Map.GetLength(0); x++)
        {
            for (var y = 0; y < distances.Map.GetLength(1); y++)
            {
                if (!camera.TileIsVisible(new MapCoord(x,y)))
                {
                    continue;
                }

                if (distances.Map[x,y].IsDefined)
                {
                    var text = new TextDraw
                    {
                        Text = distances.Map[x,y].Value?.ToString() ?? string.Empty,
                        FontSize = 14,
                        Color = Color.GreenYellow
                    };

                    textRenderer.DrawTextCentered(text, camera.MapCoordToScreenSpaceTopLeft(new(x,y)));
                }
                else
                {
                    textRenderer.DrawTextCentered(new TextDraw()
                    {
                        Color = Color.GreenYellow,
                        FontSize = 14,
                        Text = "None"
                    }, camera.MapCoordToScreenSpaceTopLeft(new(x,y)));
                }
            }
        }
    }
}