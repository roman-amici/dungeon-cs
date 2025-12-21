using Ecs;

namespace Drawing;

public struct SpriteKey<T>(T tile) where T : Enum
{
    public T Tile { get; } = tile;
}
