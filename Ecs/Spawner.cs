namespace Ecs;

public abstract class SpawningSystem<TContext>(World world) : GameSystem
{

    public World World {get;} = world;

    protected void SpawnEntity(TContext context)
    {
        Spawn(World.SpawnEntity(), context);
    }

    protected abstract void Spawn(EntityId entityId, TContext context);
}