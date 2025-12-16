namespace Map;

public class DungeonMap<T> where T : struct
{
    public T[,] Map { get; }

    public uint Width => (uint)Map.GetLength(0);
    public uint Height => (uint)Map.GetLength(1);

    public DungeonMap(uint width, uint height)
    {
        Map = new T[width, height];
    }

    public T? SafeGet(uint x, uint y)
    {
        if (x >= Map.GetLength(0) || x < 0)
        {
            return null;
        }

        if (y >= Map.GetLength(1) || y < 0)
        {
            return null;
        }

        return Map[x, y];
    }

    public T? SafeGet(MapCoord coord)
    {
        return SafeGet(coord.X,coord.Y);
    }

    public T? Up(uint x, uint y)
    {
        return SafeGet(x, y + 1);
    }

    public T? Down(uint x, uint y)
    {
        return SafeGet(x, y - 1);
    }

    public T? Right(uint x, uint y)
    {
        return SafeGet(x + 1, y);
    }

    public T? Left(uint x, uint y)
    {
        return SafeGet(x - 1, y);
    }

    public IList<T> SafeCardinals(uint x, uint y)
    {
        var l = new List<T>(4);

        var up = Up(x, y);
        var down = Down(x, y);
        var left = Left(x, y);
        var right = Right(x, y);

        if (up != null)
        {
            l.Add(up.Value);
        }

        if (down != null)
        {
            l.Add(down.Value);
        }

        if (left != null)
        {
            l.Add(left.Value);
        }

        if (right != null)
        {
            l.Add(right.Value);
        }

        return l;
    }

    public MapCoord Center => new MapCoord(Width / 2, Height / 2);
}