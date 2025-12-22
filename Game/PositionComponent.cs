using Map;

namespace Game;

public struct MapPosition(MapCoord mapPosition)
{
    public MapCoord Coord { get; set; } = mapPosition;
}

public struct UITarget(Rect2D location)
{
    public Rect2D Location {get; set;} = location;
}

public struct UseItemBehavior(ItemType itemType)
{
    public ItemType ItemType {get;} = itemType;
}