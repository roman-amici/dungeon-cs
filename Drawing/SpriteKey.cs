using Ecs;

namespace Drawing;

public struct SpriteKey<T>() where T : Enum
{
    public static readonly ComponentType TypeKey = new();

    public required T Tile { get; set; }
}
