using Ecs;
using Game;
using Map;

namespace Drawing;

public enum SpriteTile
{
    Knight,
    Goblin,
    Orc,
    Entin,
    Ogre,

    Amulet,
    Sword,
    Potion

}

public class DrawSpriteSystem(
    Camera camera,
    TileAtlas<SpriteTile> spriteAtlas,
    Screen screen,
    TableJoin<SpriteKey<SpriteTile>, Position> sprites) : GameSystem
{
    public override void Execute()
    {
        foreach (var (sprite, spritePosition) in sprites)
        {
            var topLeft = camera.MapCoordToScreenSpaceTopLeft(spritePosition.MapPosition);
            if (camera.TileIsVisible(topLeft))
            {
                spriteAtlas.DrawTile(screen, sprite.Tile, topLeft);
            }
        }
    }
}