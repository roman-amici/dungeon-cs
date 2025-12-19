using System.Collections.Concurrent;
using Map;

public abstract class Map2D<T> where T : struct
{
    public Map2D(uint width, uint height)
    {
        Map = new T[width, height];
    }

    public uint Width => (uint)Map.GetLength(0);
    public uint Height => (uint)Map.GetLength(1);
    public T[,] Map { get; }
    public MapCoord Center => new MapCoord(Width / 2, Height / 2);

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

    public IList<MapCoord> SafeCardinalCoords(MapCoord coord)
    {
        var coords = new List<MapCoord>();

        if (coord.X >= Map.GetLength(0) || coord.Y >= Map.GetLength(1))
        {
            return coords;
        }

        if (coord.X + 1 < Map.GetLength(0) )
        {
            coords.Add(new(coord.X + 1, coord.Y));
        }

        if (coord.X > 0)
        {
            coords.Add(new(coord.X - 1, coord.Y));
        }

        if (coord.Y + 1 < Map.GetLength(1))
        {
            coords.Add(new(coord.X, coord.Y + 1));
        }

        if (coord.Y > 0)
        {
            coords.Add(new(coord.X, coord.Y-1));
        }

        return coords;
    }

    public void SetAll(T value)
    {
        for (var x = 0; x < Map.GetLength(0); x++)
        {
            for (var y = 0; y < Map.GetLength(1); y++)
            {
                Map[x,y] = value;
            }
        }
    }

}