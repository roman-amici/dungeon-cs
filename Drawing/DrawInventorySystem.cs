using System.Drawing;
using Ecs;
using Game;

namespace Drawing;

public class DrawInventorySystem(
    PlayerInventory inventory,
    TableJoin<UITarget, SpriteKey<SpriteTile>> targets,
    TileAtlas<SpriteTile> sprites,
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

        foreach( var (target, sprite) in targets.Components())
        {
            sprites.DrawTile(screen, sprite.Value.Tile, target.Value.Location.TopLeft);

            var entry = inventory.Items.FirstOrDefault(x => x.Entity == target.EntityId);
            text.DrawText(new(entry.Count.ToString(), 12, Color.Black), target.Value.Location.TopLeft);
        }
    }
}