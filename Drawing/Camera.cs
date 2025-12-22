using Map;

namespace Drawing;

public class ViewPort(uint widthPixels, uint heightPixels)
{
    public uint WidthPixels { get; set; } = widthPixels;
    public uint HeightPixels { get; set; } = heightPixels;

    public Point2D Center {get;} = new(widthPixels / 2, heightPixels / 2);
}

public class Camera(ViewPort viewPort, Point2D topLeft, uint tileSize)
{
    public ViewPort ViewPort { get; } = viewPort;

    public uint TileSize { get; } = tileSize;

    public double LeftX => TopLeft.X;
    public double RightX => TopLeft.X + ViewPort.WidthPixels / TileSize;
    public double TopY => TopLeft.Y;
    public double BottomY => TopLeft.Y + ViewPort.HeightPixels / TileSize;

    public Point2D TopLeft { get; set; } = topLeft;
    public Point2D TopRight => new Point2D(RightX, TopLeft.Y);
    public Point2D BottomLeft => new Point2D(TopLeft.X, TopLeft.Y);
    public Point2D BottomRight => new Point2D(TopRight.X, BottomY);

    public Point2D MapCoordToScreenSpaceTopLeft(MapCoord coord)
    {
        var xScreen = (coord.X - TopLeft.X) * TileSize;
        var yScreen = (coord.Y - TopLeft.Y) * TileSize;

        return new Point2D(xScreen, yScreen);
    }

    public bool TileIsVisible(MapCoord coord)
    {
        var screenSpace = MapCoordToScreenSpaceTopLeft(coord);

        return TileIsVisible(screenSpace);
    }

    public bool TileIsVisible(Point2D tileTopLeft)
    {
        return tileTopLeft.X < ViewPort.WidthPixels && tileTopLeft.Y < ViewPort.HeightPixels;
    }

    public void SetCenter(MapCoord center)
    {
        var x = center.X - (ViewPort.WidthPixels / TileSize / 2);
        var y = center.Y - (ViewPort.HeightPixels / TileSize / 2);

        TopLeft = new (x,y);
    }

    internal MapCoord? ScreenSpaceToMapCoord(Point2D screenSpace)
    {
        var xMap = Math.Floor(TopLeft.X + screenSpace.X / TileSize);
        var yMap = Math.Floor(TopLeft.Y + screenSpace.Y / TileSize);

        if (xMap < 0 || yMap < 0)
        {
            return null;
        }

        return new MapCoord((uint)xMap,(uint)yMap);
    }
}