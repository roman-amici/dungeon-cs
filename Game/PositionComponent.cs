using Map;

namespace Game;

public struct Position(MapCoord MapPosition)
{
    public MapCoord MapPosition { get; set; } = MapPosition;
}