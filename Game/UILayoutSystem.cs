using System.Drawing;
using Drawing;
using Ecs;

namespace Game;

public class UILayoutSystem(
    UILayout layout,
    PlayerInventory inventory,
    TileAtlas<SpriteTile> sprites) : GameSystem
{
    public override void Execute()
    {
        if (!layout.IsDirty)
        {
            return;
        }

        layout.UIObjects.Clear();

        var x = DrawInventorySystem.xPadding;
        var y = DrawInventorySystem.yPadding;
        foreach(var entry in inventory.Items)
        {
            var rect = new Rect2D(new(x,y), new(x + sprites.TileScreenSize, y + sprites.TileScreenSize));
            var tile = ItemSpawner.GetTile(entry.ItemType);
            layout.UIObjects.Add(new SpriteButton(rect, new UseItemAction(entry.ItemType), tile, entry.Count.ToString()));

            x += (int)sprites.TileScreenSize;
        }

        layout.IsDirty = false;
    }
}

public class UILayout
{
    public List<UIObject> UIObjects {get; } = [];

    public bool IsDirty {get; set;} = true;
}

public abstract class UIObject(UITarget? target, PlayerAction? action)
{
    public UITarget? Target {get;} = target;
    public PlayerAction? Action {get;} = action;

    public abstract void Draw(TileAtlas<SpriteTile> tileAtlas, TextRenderer text, Screen screen);
}

public class SpriteButton(Rect2D target, PlayerAction? action, SpriteTile tile, string count) : UIObject(new(target), action)
{
    public override void Draw(TileAtlas<SpriteTile> tileAtlas, TextRenderer text, Screen screen)
    {
        if (Target == null)
        {
            return;
        }

        tileAtlas.DrawTile(screen, tile, Target.Value.Location.TopLeft);
        text.DrawText(new(count, 12, Color.Black), Target.Value.Location.TopLeft);
    }
}

public struct UITarget(Rect2D location)
{
    public Rect2D Location {get; set;} = location;
}