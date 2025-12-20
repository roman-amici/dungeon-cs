using System.ComponentModel;

namespace Ecs;

public class Singleton<T> : IComponentContainer where T : struct
{
    public Singleton()
    {
    }

    public void Spawn(EntityId entityId, T value)
    {
        if (Singlet != null)
        {
            throw new InvalidOperationException("Singleton already spawned");
        }

        SingletComponent = new Component<T>(entityId,value);
    }

    public T? Singlet => SingletComponent?.Value;

    public Component<T>? SingletComponent {get; private set;}

    public void RemoveEntity(EntityId entityId)
    {
        if (SingletComponent?.EntityId == entityId)
        {
            SingletComponent = null;
        }
    }
}