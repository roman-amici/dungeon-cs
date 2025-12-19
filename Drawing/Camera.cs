using Map;

namespace Drawing;

public class ViewPort(uint widthPixels, uint heightPixels)
{
    public uint WidthPixels { get; set; } = widthPixels;
    public uint HeightPixels { get; set; } = heightPixels;

    public Point2D Center {get;} = new(widthPixels / 2, heightPixels / 2);
}

public class Camera(ViewPort viewPort, MapCoord topLeft, uint tileSize)
{
    public ViewPort ViewPort { get; } = viewPort;

    public uint TileSize { get; } = tileSize;

    public uint LeftX => TopLeft.X;
    public uint RightX => TopLeft.X + ViewPort.WidthPixels / TileSize;
    public uint TopY => TopLeft.Y;
    public uint BottomY => TopLeft.Y + ViewPort.HeightPixels / TileSize;

    public MapCoord TopLeft { get; set; } = topLeft;
    public MapCoord TopRight => new MapCoord(RightX, TopLeft.Y);
    public MapCoord BottomLeft => new MapCoord(TopLeft.X, TopLeft.Y);
    public MapCoord BottomRight => new MapCoord(TopRight.X, BottomY);

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

    internal MapCoord ScreenSpaceToMapCoord(Point2D screenSpace)
    {
        var xMap = TopLeft.X + (uint)Math.Floor(screenSpace.X / TileSize);
        var yMap = TopLeft.Y + (uint)Math.Floor(screenSpace.Y / TileSize);

        return new MapCoord(xMap,yMap);
    }
}