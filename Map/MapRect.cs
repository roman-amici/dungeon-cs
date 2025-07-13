namespace Map;

public struct MapRect(uint x, uint y, uint width, uint height)
{
    public uint X { get; } = x;
    public uint Y { get; } = y;

    public (uint Start, uint End) IntervalX => (X, X + Width - 1);
    public (uint Start, uint End) IntervalY => (Y, Y + Height - 1);

    public MapCoord Center => new MapCoord(X + Width / 2, Y + Height / 2);

    public uint Width { get; } = width;
    public uint Height { get; } = height;

    public bool IntervalOverlaps((uint a1, uint a2) ta, (uint b1, uint b2) tb)
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