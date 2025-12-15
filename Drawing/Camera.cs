using Map;

namespace Drawing;

public class ViewPort(uint widthPixels, uint heightPixels)
{
    public uint WidthPixels { get; set; } = widthPixels;
    public uint HeightPixels { get; set; } = heightPixels;
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

    public Point2D ScreenSpaceTileTopLeft(MapCoord coord)
    {
        var xScreen = (coord.X - TopLeft.X) * TileSize;
        var yScreen = (coord.Y - TopLeft.Y) * TileSize;

        return new Point2D(xScreen, yScreen);
    }

    public bool TileIsVisible(MapCoord coord)
    {
        var screenSpace = ScreenSpaceTileTopLeft(coord);

        return TileIsVisible(screenSpace);
    }

    public bool TileIsVisible(Point2D tileTopLeft)
    {
        return (tileTopLeft.X > 0 && tileTopLeft.Y > 0) ||
        (tileTopLeft.X > 0 && tileTopLeft.Y + TileSize > 0) ||
        (tileTopLeft.X + TileSize > 0 && tileTopLeft.Y > 0) ||
        (tileTopLeft.X + TileSize > 0 && tileTopLeft.Y + TileSize > 0);
    }

    public void SetCenter(MapCoord center)
    {
        var x = center.X - (ViewPort.WidthPixels / TileSize / 2);
        var y = center.Y - (ViewPort.HeightPixels / TileSize / 2);

        TopLeft = new (x,y);
    }
}