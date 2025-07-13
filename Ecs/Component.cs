using System.Diagnostics.CodeAnalysis;

namespace Ecs;

public struct Component<T> where T : struct
{
    public Component(ulong entityId, T value)
    {
        EntityId = entityId;
        Value = value;
    }

    public readonly ulong EntityId;
    public readonly T Value;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Component<T> c)
        {
            return c.EntityId == EntityId;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return EntityId.GetHashCode();
    }
}