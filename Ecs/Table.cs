using System.Collections;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace Ecs;

// Base table not optimized for insertions or removals
public class Table<T> : IComponentContainer, IEnumerable<T>
    where T : struct
{
    private List<Component<T>> data = new();

    public int Count => data.Count;

    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < Count; i++)
        {
            yield return data[i].Value;
        }
    }

    public IEnumerable<Component<T>> Components()
    {
        for (var i = 0; i < Count; i++)
        {
            yield return data[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Component<T> value)
    {

        for (var i = 0; i < data.Count; i++)
        {
            if(value.EntityId == data[i].EntityId)
            {
                throw new InvalidOperationException($"Entity {value.EntityId} already added.");
            }

            if (value.EntityId.Id > data[i].EntityId.Id)
            {
                data.Insert(i, value);
                return;
            }
        }

        data.Add(value);
    }

    public void Add(EntityId entityId, T value)
    {
        Add(new Component<T>(entityId, value));
    }

    public void Update(int i, T value)
    {
        var component = data[i];
        data[i] = new Component<T>(component.EntityId, value);
    }

    public int Update(EntityId entityId, T value)
    {
        for(var i = 0; i < data.Count; i++)
        {
            if (data[i].EntityId == entityId)
            {
                Update(i,value);

                return i;
            }
        }

        return -1;
    }

    public T this[int i]
    {
        get => data[i].Value;
    }

    public T this[EntityId entityId]
    {
        get
        {
            var e = Find(entityId);
            if (e == null)
            {
                throw new IndexOutOfRangeException($"No entity {entityId} in table");
            }

            return e.Value;
        }
    }

    public Component<T> GetComponent(int i)
    {
        return data[i];
    }

    public T? Find(EntityId entityId)
    {
        for(var i = 0; i < data.Count; i++)
        {
            if (data[i].EntityId == entityId)
            {
                return data[i].Value;
            }
        }

        return null;
    }

    public Component<T>? FindComponent(EntityId entityId)
    {
        for(var i = 0; i < data.Count; i++)
        {
            if (data[i].EntityId == entityId)
            {
                return data[i];
            }
        }

        return null;
    }

    public bool Remove(EntityId entityId)
    {
        for (var i = 0; i < data.Count; i++)
        {
            if (data[i].EntityId == entityId)
            {
                data.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public T? FindWhere(Func<T, bool> predicate)
    {
        foreach(var component in data)
        {
            if (predicate(component.Value))
            {
                return component.Value;
            }
        }

        return null;
    }

    public Component<T>? FindWhereComponent(Func<Component<T>,bool> predicate)
    {
        foreach(var component in data)
        {
            if (predicate(component))
            {
                return component;
            }
        }

        return null;
    }

     void IComponentContainer.RemoveEntity(EntityId entityId)
    {
        Remove(entityId);
    }
}