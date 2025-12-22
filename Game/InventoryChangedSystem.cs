using Drawing;
using Ecs;

namespace Game;

public class InventoryChangedSystem(
    World world,
    Queue<InventoryChangeMessage> messageQueue,
    UILayout changeLayout,
    PlayerInventory inventory,
    Table<UITarget> targets,
    Table<SpriteKey<SpriteTile>> itemSprites,
    Table<UseItemBehavior> useItem
    ) : GameSystem
{
    public override void Execute()
    {
        while (messageQueue.TryDequeue(out var message))
        {
            if (message.ChangeType == InventoryChangeType.Add)
            {
                if (!inventory.TryIncrementCount(message.ItemType))
                {
                    var entityId = world.SpawnEntity();
                    inventory.Items.Add(new(entityId, message.ItemType, 1));
                    targets.Add(entityId, new());
                    itemSprites.Add(entityId, new(ItemSpawner.GetTile(message.ItemType)));
                    useItem.Add(entityId, new(message.ItemType));
                    changeLayout.IsDirty = true;
                }
            }
            else if (message.ChangeType == InventoryChangeType.Remove)
            {
                inventory.TryDecrementCount(message.ItemType);
            }
        }

        var toRemove = new List<EntityId>(0);
        foreach(var item in inventory.Items)
        {
            if (item.Count == 0)
            {
                toRemove.Add(item.Entity);
                changeLayout.IsDirty = true;
            }
        }

        foreach(var entityId in toRemove)
        {
            world.RemoveEntity(entityId);
            inventory.Items.RemoveAll(x => x.Entity == entityId);
        }
    }
}

public enum InventoryChangeType
{
    Add,
    Remove
}

public struct InventoryChangeMessage(InventoryChangeType changeType, ItemType item)
{
    public InventoryChangeType ChangeType {get; } = changeType;

    public ItemType ItemType {get;} = item;
}