using System.Globalization;

namespace Map;

public class DungeonMap<T> : Map2D<T>
where T : struct
{
    public DungeonMap(uint width, uint height) : base(width,height){}

    public DungeonMap(T[,] map) : base(map){}

    public MapCoord PlayerStart {get; set;}
    public List<MapCoord> EnemySpawns {get;} = [];

}