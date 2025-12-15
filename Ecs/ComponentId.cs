using System.Diagnostics.CodeAnalysis;

namespace Ecs;

public struct ComponentId(ulong id)
{
    public ulong Id {get;} = id;

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ComponentId otherId)
        {
            return otherId.Id == Id;
        }

        return false;
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}