using Ecs;

namespace Game;

public class PickupItemSystem(
    World world,
    SingletonJoin<Player, MapPosition> playerPosition,
    TableJoin<PickupItem, MapPosition> itemPosition,
    Queue<InventoryChangeMessage> inventoryChangeQueue) : GameSystem
{
    public override void Execute()
    {
        var pPosition = playerPosition.Join;
        if (pPosition == null)
        {
            return;
        }

        var (_,player) = pPosition.Value;

        var itemsToRemove = new List<EntityId>(1);
        foreach(var (item,itemPosition) in itemPosition.Components())
        {
            if (itemPosition.Value.Coord == player.Coord)
            {
                inventoryChangeQueue.Enqueue(new(InventoryChangeType.Add, item.Value.ItemType));
                itemsToRemove.Add(item.EntityId);
            }
        }

        foreach(var entityId in itemsToRemove)
        {
            world.RemoveEntity(entityId);
        }

    }
}