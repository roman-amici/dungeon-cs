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
        this.t1 = t1;
        this.t2 = t2;
    }

    private readonly Table<T> t1;
    private readonly Table<U> t2;

    public IEnumerator<(Component<T>, Component<U>)> GetEnumerator()
    {
        var lastJ = 0;
        for (var i = 0; i < t1.Count; i++)
        {
            for (var j = lastJ; j < t2.Count; j++)
            {
                if (t1[i].EntityId == t2[i].EntityId)
                {
                    yield return (t1[i], t2[i]);

                    lastJ = j+1;
                    break;
                }
            }

            if (lastJ >= t2.Count)
            {
                yield break;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}