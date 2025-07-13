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
}