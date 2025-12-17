using Ecs;

namespace Game;

public class KillEntitiesSystem(World world, Table<Health> healths) : GameSystem
{
    public override void Execute()
    {
        var toKill = new List<EntityId>();
        foreach(var health in healths)
        {
            if (health.Value.CurrentHealth < 0)
            {
                toKill.Add(health.EntityId);
            }
        }

        foreach(var entityId in toKill)
        {
            world.RemoveEntity(entityId);
        }
    }
}