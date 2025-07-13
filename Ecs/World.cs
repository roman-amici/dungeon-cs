using Ecs;

public class World()
{
    private ulong NextEntityId { get; set; } = 1;

    public HashSet<ulong> Entities { get; } = [];
    public List<IComponentContainer> Containers { get; } = [];

    public List<GameSystem> Systems { get; } = [];

    public void Execute()
    {
        foreach (var system in Systems)
        {
            system.Execute();
        }
    }

    public void RemoveEntity(ulong entityId)
    {
        Entities.Remove(entityId);
        foreach (var container in Containers)
        {
            container.RemoveEntity(entityId);
        }
    }

    public ulong SpawnEntity(Spawner spawner)
    {
        var entityId = NextEntityId++;

        Entities.Add(entityId);

        spawner.Spawn(entityId);

        return entityId;
    }

}