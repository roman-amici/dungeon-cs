namespace Map;

public class DungeonMap<T>(uint width, uint height) : Map2D<T>(width, height)
where T : struct
{
    public List<MapRect> Rooms {get;} = new();

}