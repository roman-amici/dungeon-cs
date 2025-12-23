using Map;

namespace Game;

public struct MapPosition(MapCoord mapPosition)
{
    public MapCoord Coord { get; set; } = mapPosition;
}