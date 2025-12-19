using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Ecs;

public class TableJoin<T, U> : IEnumerable<(Component<T>, Component<U>)>
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

    public IEnumerator<(Component<T>, Component<U>)> GetEnumerator()
    {
        foreach (var (i,j) in GetIndices())
        {
            yield return (T1[i],T2[j]);
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
                if (T1[i].EntityId == T2[j].EntityId)
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

    public (Component<T>,Component<U>)? FirstWhere(Func<(T,U),bool> predicate)
    {
        foreach (var (i,j) in GetIndices())
        {
            if (predicate((T1[i].Value,T2[j].Value)))
            {
                return (T1[i],T2[j]);
            }
        }

        return null;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public (T,U) this[int indexT, int indexU]
    {
        get
        {
            return (T1[indexT].Value,T2[indexU].Value);
        }

    }
}