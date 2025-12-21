using System.Collections;

namespace Ecs;

public class SingletonJoin<TSingle,TTable> : IComponentJoin
    where TSingle : struct
    where TTable : struct
{
    public SingletonJoin(Singleton<TSingle> single, Table<TTable> table)
    {
        S = single;
        T = table;
    }

    public Singleton<TSingle> S {get;}
    public Table<TTable> T {get;}

    public (TSingle,TTable)? Join
    {
        get
        {
            if (S.SingletComponent == null)
            {
                return null;
            }

            var t = T.Find(S.SingletComponent.Value.EntityId);

            if (t == null)
            {
                return null;
            }

            return (S.SingletComponent.Value.Value, t.Value);
        }
    }

        public (Component<TSingle>,Component<TTable>)? JoinComponent
    {
        get
        {
            if (S.SingletComponent == null)
            {
                return null;
            }

            var t = T.FindComponent(S.SingletComponent.Value.EntityId);

            if (t == null)
            {
                return null;
            }

            return (S.SingletComponent.Value, t.Value);
        }
    }
}
