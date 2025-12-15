using System.Collections;

namespace Ecs;

public class SingletonJoin<TSingle,TTable> : IEnumerable<(Component<TSingle>, Component<TTable>)>
    where TSingle : struct
    where TTable : struct
{
    public SingletonJoin(Singleton<TSingle> single, Table<TTable> table)
    {
        Single = single;
        Table = table;
    }

    public Singleton<TSingle> Single {get;}
    public Table<TTable> Table {get;}

    public IEnumerator<(Component<TSingle>, Component<TTable>)> GetEnumerator()
    {
        if (Single.First == null)
        {
            yield break;
        }

        for (var i = 0; i < Table.Count; i++)
        {
            if (Table[i].EntityId == Single.First.Value.EntityId)
            {
                yield return (Single.First.Value, Table[i]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
