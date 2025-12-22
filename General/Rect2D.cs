
public struct Rect2D
{
    public Rect2D()
    {
    }

    public Rect2D(Point2D topLeft, Point2D bottomRight)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
    }

    public Point2D TopLeft { get; set; }
    public Point2D BottomRight { get; set; }

    public bool Intersects(Rect2D other)
    {
        if (TopLeft.X < other.BottomRight.X && BottomRight.X > other.TopLeft.X &&
        TopLeft.Y > other.BottomRight.Y && BottomRight.Y < other.TopLeft.Y )
        {
            return true;
        } 

        return false;
    }

    public bool Contains(Point2D point)
    {
        return TopLeft.X <= point.X &&
            TopLeft.Y <= point.Y &&
            BottomRight.X >= point.X &&
            BottomRight.Y >= point.Y;
    }
}