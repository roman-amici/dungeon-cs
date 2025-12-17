using Ecs;

namespace Drawing;

public struct SpriteKey<T>(T tile) where T : Enum
{
    public static readonly ComponentType TypeKey = new();

    public T Tile { get; } = tile;
}
