using Ecs;

public class World()
{
    private ulong NextEntityId { get; set; } = 1;

    public HashSet<EntityId> Entities { get; } = [];
    public Dictionary<Type, IComponentContainer> Components { get; } = [];
    public Dictionary<Type, object> Resources {get;} = [];
    public Dictionary<Type, IComponentJoin> Joins {get;} = [];

    public void RemoveEntity(EntityId entityId)
    {
        Entities.Remove(entityId);
        foreach (var container in Components.Values)
        {
            container.RemoveEntity(entityId);
        }
    }

    public EntityId SpawnEntity(Spawner spawner, object? context)
    {
        var entityId = new EntityId(NextEntityId++);

        Entities.Add(entityId);

        spawner.Spawn(entityId, context);

        return entityId;
    }

    public void AddComponent(IComponentContainer component)
    {
        if(!Components.TryAdd(component.GetType(), component))
        {
            throw new InvalidOperationException($"Already added component with type {component.GetType()}");
        }
    }

    public void AddResource(object resource)
    {
        if (!Resources.TryAdd(resource.GetType(), resource))
        {
            throw new InvalidOperationException($"Already added resource with type {resource.GetType()}");
        }
    }

    public void AddTableJoin<T,U>()
    where T : struct
    where U : struct
    {
        var joinType = typeof(TableJoin<T,U>);
        if (Joins.ContainsKey(joinType))
        {
            return;
        }

        if (!Components.TryGetValue(typeof(Table<T>), out var table1))
        {
            throw new Exception($"Unknown table component {typeof(T)}");
        }

        if (!Components.TryGetValue(typeof(Table<U>), out var table2))
        {
            throw new Exception($"Unknown table component {typeof(T)}");
        }

        Joins.Add(joinType, new TableJoin<T,U>((Table<T>)table1,(Table<U>)table2));
    }

    public void AddSingletonJoin<T,U>()
    where T : struct
    where U : struct
    {
        var joinType = typeof(SingletonJoin<T,U>);
        if (Joins.ContainsKey(joinType))
        {
            return;
        }

        if (!Components.TryGetValue(typeof(Singleton<T>), out var singleton))
        {
            throw new Exception($"Unknown singleton component {typeof(T)}");
        }

        if (!Components.TryGetValue(typeof(Table<U>), out var table))
        {
            throw new Exception($"Unknown table component {typeof(T)}");
        }

        Joins.Add(joinType, new SingletonJoin<T,U>((Singleton<T>)singleton,(Table<U>)table));
    }

    public T CreateSystem<T>()
    where T : GameSystem
    {
        var systemType = typeof(T);

        var constructors = systemType.GetConstructors();
        if (constructors.Length > 1)
        {
            throw new InvalidOperationException("Game system has more than one constructor");
        }

        var constructor = constructors.First();

        var parameterValues = new List<object>();
        foreach( var parameter in constructor.GetParameters())
        {
            var parameterType = parameter.ParameterType;
            if (GetType() == parameterType)
            {
                parameterValues.Add(this);
            }
            else if (Components.TryGetValue(parameterType, out var component))
            {
                parameterValues.Add(component);
            }
            else if (Resources.TryGetValue(parameterType, out var resource))
            {
                parameterValues.Add(resource);
            }
            else if(Joins.TryGetValue(parameterType, out var join))
            {
                parameterValues.Add(join);
            }
            else
            {
                throw new InvalidOperationException($"Missing parameter {parameterType}");
            }
        }

        var instance = constructor.Invoke(parameterValues.ToArray());
        if (instance is not T gs)
        {
            throw new InvalidCastException($"Failed to create system {systemType}");
        }

        return gs;
    }

}