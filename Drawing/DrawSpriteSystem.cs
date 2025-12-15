using Ecs;
using Game;
using Map;

namespace Drawing;

public enum SpriteTile
{
    Knight
}

public class DrawSpriteSystem(
    DungeonMap<MapTile> map,
    Camera camera,
    TileAtlas<SpriteTile> spriteAtlas,
    Screen screen,
    TableJoin<SpriteKey<SpriteTile>, Position> sprites) : GameSystem
{
    public override void Execute()
    {
        foreach (var (sprite, spritePosition) in sprites)
        {
            var topLeft = camera.ScreenSpaceTileTopLeft(spritePosition.Value.MapPosition);
            if (camera.TileIsVisible(topLeft))
            {
                spriteAtlas.DrawTile(screen, sprite.Value.Tile, topLeft);
            }
        }
    }
}