namespace Map;

public struct MapCoord(uint x, uint y)
{
    public uint X { get; } = x;
    public uint Y { get; } = y;

    public uint DistanceX(MapCoord other)
    {
        return (uint)Math.Abs(other.X - X);
    }

    public uint DistanceY(MapCoord other)
    {
        return (uint)Math.Abs(other.Y - Y);
    }

    public uint DistanceManhattan(MapCoord other)
    {
        return DistanceX(other) + DistanceY(other);
    }

    public double DistanceCartesian(MapCoord other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }

    public MapCoord? SafeUp()
    {
        if (Y == 0)
        {
            return null;
        }
        return new(X,Y-1);
    }

    public MapCoord Down()
    {
        return new(X,Y+1);
    }

    public MapCoord? SafeLeft()
    {
        if (X == 0)
        {
            return null;
        }
        return new(X-1,Y);
    }

    public MapCoord Right()
    {
        return new(X+1,Y);
    }
}