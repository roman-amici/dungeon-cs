using System.Diagnostics.CodeAnalysis;

namespace Map;

public struct MapCoord
{
    public MapCoord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public MapCoord(MapCoord coord)
    {
        X = coord.X;
        Y = coord.Y;
    }

    public int X { get; }
    public int Y { get; }

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

    public MapCoord Up()
    {
        return new(X,Y-1);
    }

    public MapCoord Down()
    {
        return new(X,Y+1);
    }

    public MapCoord Left()
    {
        return new(X-1,Y);
    }

    public MapCoord Right()
    {
        return new(X+1,Y);
    }

    public Point2D ToPoint()
    {
        return new Point2D(X,Y);
    }

    public static bool operator ==(MapCoord left, MapCoord right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MapCoord left, MapCoord right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is MapCoord coord)
        {
            return coord.X == X && coord.Y == Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return (X,Y).GetHashCode();
    }
}