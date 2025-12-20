using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Ecs;

public class TableJoin<T, U> : IEnumerable<(T, U)>
where T : struct
where U : struct

{
    public TableJoin(Table<T> t1, Table<U> t2)
    {
        T1 = t1;
        T2 = t2;
    }

    public Table<T> T1 {get;}
    public Table<U> T2 {get;}


    public (T,U) this[int indexT, int indexU]
    {
        get
        {
            return (T1[indexT],T2[indexU]);
        }
    }

    public (T,U) this[EntityId entityId]
    {
        get
        {
            var t = T1[entityId];
            var u = T2[entityId];

            return (t,u);
        }
    }

    public (T,U)? Find(EntityId entityId)
    {
        var t = T1.Find(entityId);
        if (t == null)
        {
            return null;
        }

        var u = T2.Find(entityId);
        if (u == null)
        {
            return null;
        }

        return (t.Value,u.Value);
    }

    public IEnumerator<(T, U)> GetEnumerator()
    {
        foreach (var (i,j) in GetIndices())
        {
            yield return (T1[i],T2[j]);
        }
    }

    public IEnumerable<(Component<T>,Component<U>)> Components()
    {
        foreach (var (i,j) in GetIndices())
        {
            yield return (T1.GetComponent(i),T2.GetComponent(j));
        }
    }

    public IEnumerable<(int,int)> GetIndices()
    {
        // Optimization since we know that tables are in sorted order
        var lastJ = 0;
        for (var i = 0; i < T1.Count; i++)
        {
            for (var j = lastJ; j < T2.Count; j++)
            {
                if (T1.GetComponent(i).EntityId == T2.GetComponent(j).EntityId)
                {
                    yield return (i,j);

                    lastJ = j+1;
                    break;
                }
            }

            if (lastJ >= T2.Count)
            {
                yield break;
            }
        }
    }

    public (T,U)? FindWhere(Func<(T,U),bool> predicate)
    {
        foreach (var (i,j) in GetIndices())
        {
            if (predicate((T1[i],T2[j])))
            {
                return (T1[i],T2[j]);
            }
        }

        return null;
    }

    public (Component<T>,Component<U>)? FindComponentWhere(Func<(T,U),bool> predicate)
    {
        foreach (var (i,j) in GetIndices())
        {
            if (predicate((T1[i],T2[j])))
            {
                return (T1.GetComponent(i),T2.GetComponent(j));
            }
        }

        return null;
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}