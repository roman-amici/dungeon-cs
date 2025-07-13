using Map;
using SdlAbstractions;

namespace Drawing;

public class TileAtlas(Texture tileSheet, uint tileScreenSize)
{
    public Texture TileSheet { get; } = tileSheet;

    public Dictionary<Tile, Rect2D> TileCoordinates { get; } = new();

    public uint TileScreenSize { get; } = tileScreenSize;

    public void AddGridTile(Tile tile, uint tileSize, uint x, uint y)
    {
        var topLeft = new Point2D(x * tileSize, y * tileSize);
        var bottomRight = new Point2D(topLeft.X + tileSize, topLeft.Y + tileSize);

        TileCoordinates.Add(tile, new Rect2D(topLeft, bottomRight));
    }

    public void DrawTile(Screen screen, Tile tile, Point2D topLeft)
    {
        if (!TileCoordinates.TryGetValue(tile, out var textureCoords))
        {
            textureCoords = TileCoordinates.First().Value;
        }

        var screenTile = new Rect2D
        {
            TopLeft = topLeft,
            BottomRight = new(topLeft.X + TileScreenSize, topLeft.Y + TileScreenSize)
        };

        screen.DrawTexture(TileSheet, textureCoords, screenTile);
    }
}