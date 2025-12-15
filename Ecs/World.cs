using System.ComponentModel;
using Ecs;

public class World()
{
    private ulong NextEntityId { get; set; } = 1;

    private ulong NextComponentId {get; set;} = 1;

    public HashSet<EntityId> Entities { get; } = [];
    public Dictionary<ComponentId, IComponentContainer> Containers { get; } = [];
    public List<GameSystem> Systems { get; } = [];

    public void Execute()
    {
        foreach (var system in Systems)
        {
            system.Execute();
        }
    }

    public void RemoveEntity(EntityId entityId)
    {
        Entities.Remove(entityId);
        foreach (var container in Containers.Values)
        {
            container.RemoveEntity(entityId);
        }
    }

    public EntityId SpawnEntity(Spawner spawner)
    {
        var entityId = new EntityId(NextEntityId++);

        Entities.Add(entityId);

        spawner.Spawn(entityId);

        return entityId;
    }

    public ComponentId AddComponent(IComponentContainer component)
    {
        var next = NextComponentId++;
        var id = new ComponentId(next);

        Containers.Add(id, component);

        return id;
    }

}