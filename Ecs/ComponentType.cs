using System.Diagnostics.CodeAnalysis;

namespace Ecs;

public struct ComponentType
{

    public ComponentType()
    {
        Id = Interlocked.Increment(ref nextComponentTypeId);
    }

    public readonly uint Id;
    private static uint nextComponentTypeId;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ComponentType t)
        {
            return Id.Equals(t.Id);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}