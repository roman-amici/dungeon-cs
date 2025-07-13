namespace Ecs;

// Base table not optimized for insertions or removals
public class Table<T> : List<Component<T>>, IComponentContainer where T : struct
{

    public new void Add(Component<T> value)
    {
        if (this.Any(x => x.EntityId == value.EntityId))
        {
            throw new InvalidOperationException($"Entity with id {value.EntityId} already exists");
        }

        base.Add(value);
    }

    public new Component<T> this[int i]
    {
        get => base[i];
        set => throw new InvalidOperationException("Use Update instead");
    }

    public void Update(Func<T, T?> updateFunction)
    {
        for (var i = 0; i < Count; i++)
        {
            var result = updateFunction(this[i].Value);
            if (result != null)
            {
                base[i] = new Component<T>(this[i].EntityId, result.Value);
            }
        }
    }

    public void RemoveEntity(ulong entityId)
    {
        var index = -1;
        for (var i = 0; i < Count; i++)
        {
            if (base[i].EntityId == entityId)
            {
                index = i;
                break;
            }
        }

        if (index > 0)
        {
            RemoveAt(index);
        }
    }
}