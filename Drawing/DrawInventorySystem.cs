using System.Drawing;
using Ecs;
using Game;

namespace Drawing;

public class DrawInventorySystem(
    TileAtlas<SpriteTile> sprites,
    UILayout layout,
    Screen screen,
    TextRenderer text,
    ViewPort viewPort) : GameSystem
{
    public const int yPadding = 2;
    public const int xPadding = 2; 

    public override void Execute()
    {
        var rect = new Rect2D(new Point2D(0,0), new Point2D(viewPort.WidthPixels, sprites.TileScreenSize + 2 * yPadding));   
        screen.DrawRect(rect, Color.Goldenrod);

        foreach(var target in layout.UIObjects)
        {
            target.Draw(sprites,text,screen);
        }
    }
}