using System.Data.SqlTypes;

public struct Point2D(double x, double y)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    public double DistanceCartesian(Point2D other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx*dx - dy*dy);
    }
}

