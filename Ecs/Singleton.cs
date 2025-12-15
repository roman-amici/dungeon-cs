using System.ComponentModel;

namespace Ecs;

public class Singleton<T> : IComponentContainer where T : struct
{
    public Singleton()
    {
    }

    public void Spawn(EntityId entityId, T value)
    {
        if (First != null)
        {
            throw new InvalidOperationException("Singleton already spawned");
        }

        First = new Component<T>(entityId,value);
    }

    public Component<T>? First {get; private set;}

    public void RemoveEntity(EntityId entityId)
    {
        if (First?.EntityId == entityId)
        {
            First = null;
        }
    }
}