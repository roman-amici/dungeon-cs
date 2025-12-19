namespace Map;

public class DistanceMap(uint width, uint height) : Map2D<Distance>(width,height)
{
    private const uint Unknown = uint.MaxValue;

    public bool IsDirty {get; set;} = true;

    public DistanceMap BuildFromMap<T>(
        MapCoord start, 
        DungeonMap<T> map,
        Func<MapCoord,bool> isFloor)
    where T : struct
    {
        var distMap = new DistanceMap(map.Width, map.Height);
        distMap.UpdateFromMap(start, isFloor);

        return distMap;
        
    }

    public void UpdateFromMap(
        MapCoord start,
        Func<MapCoord,bool> isFloor)
    {

        SetAll(new(Unknown));
        Map[start.X,start.Y] = new(0);

        var queue = new PriorityQueue<MapCoord, uint>();
        foreach (var nextCoords in SafeCardinalCoords(start))
        {
            queue.Enqueue(nextCoords, 1);
        }

        while (queue.TryDequeue(out var coord, out var distance))
        {
            if ( Map[coord.X,coord.Y].IsDefined && Map[coord.X,coord.Y].Value != Unknown)
            {
                continue;
            }

            if (!isFloor(coord))
            {
                Map[coord.X,coord.Y] = new(null);
                continue;
            }

            Map[coord.X, coord.Y] = new(distance);
            foreach (var nextCoords in SafeCardinalCoords(coord))
            {
                queue.Enqueue(nextCoords, distance + 1);
            }
        }

        // Allows us to easily find the smallest and largest values
        for(var x = 0; x < Map.GetLength(0); x++)
        {
            for(var y = 0; y < Map.GetLength(1); y++)
            {
                if(Map[x,y].Value == Unknown)
                {
                    Map[x,y] = new(null);
                }
            }
        }

        IsDirty = false;
    }

    public MapCoord? NextNearest(MapCoord coord)
    {
        if( !Map[coord.X,coord.Y].IsDefined )
        {
            return null;
        }

        return SafeCardinalCoords(coord)
            .Where(c => Map[c.X,c.Y].IsDefined)
            .MinBy(c => Map[c.X,c.Y].Value);
    }
}

public struct Distance(uint? distance)
{
    public uint? Value {get;} = distance;

    public bool IsDefined => Value != null;
}