namespace Map;

public struct MapRect(int x, int y, uint width, uint height)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public (int Start, int End) IntervalX => (X, X + (int)Width - 1);
    public (int Start, int End) IntervalY => (Y, Y + (int)Height - 1);

    public MapCoord Center => new MapCoord(X + (int)Width / 2, Y + (int)Height / 2);

    public uint Width { get; } = width;
    public uint Height { get; } = height;

    public bool IntervalOverlaps((int a1, int a2) ta, (int b1, int b2) tb)
    {
        var aMax = Math.Max(ta.a1, ta.a2);
        var aMin = Math.Min(ta.a1, ta.a2);
        var bMax = Math.Max(tb.b1, tb.b2);
        var bMin = Math.Max(tb.b1, tb.b2);
        return aMax >= bMin && bMax >= aMin;
    }


    public bool Intersects(MapRect other)
    {
        return IntervalOverlaps(IntervalX, other.IntervalX) &&
            IntervalOverlaps(IntervalY, other.IntervalY);
    }

}