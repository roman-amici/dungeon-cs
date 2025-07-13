
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
}