using Drawing;
using Ecs;

namespace Game;

public class UILayoutSystem(
    UILayout layout,
    PlayerInventory inventory,
    Table<UITarget> targets,
    TileAtlas<SpriteTile> sprites) : GameSystem
{
    public override void Execute()
    {
        if (!layout.IsDirty)
        {
            return;
        }

        var x = DrawInventorySystem.xPadding;
        var y = DrawInventorySystem.yPadding;
        foreach(var entry in inventory.Items)
        {
            var rect = new Rect2D(new(x,y), new(x + sprites.TileScreenSize, y + sprites.TileScreenSize));
            targets.Update(entry.Entity, new(rect));
            x += (int)sprites.TileScreenSize;
        }

        layout.IsDirty = false;
    }
}

public class UILayout
{
    public bool IsDirty {get; set;} = true;
}