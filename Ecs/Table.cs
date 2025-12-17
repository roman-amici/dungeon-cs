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

        for (var i = 0; i < Count; i++)
        {
            if (value.EntityId.Id > this[i].EntityId.Id)
            {
                Insert(i, value);
                return;
            }
        }

        base.Add(value);
    }

    public void Add (EntityId entityId, T value)
    {
        Add(new Component<T>(entityId, value));
    }

    public void Update(int i, T value)
    {
        var component = base[i];
        base[i] = new Component<T>(component.EntityId, value);
    }

    public void Update(EntityId id, T Value)
    {
        var i = FindIndex(x => x.EntityId == id);
        if (i >= 0)
        {
            Update(i,Value);
        }
    }

    public new Component<T> this[int i]
    {
        get => base[i];
        set => throw new InvalidOperationException("Use Update instead");
    }

    public void RemoveEntity(EntityId entityId)
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