using Drawing;
using Ecs;

namespace Game;

public class InventoryChangedSystem(
    Queue<InventoryChangeMessage> messageQueue,
    UILayout changeLayout,
    PlayerInventory inventory,
    SingletonJoin<Player,Damage> playerDamage
    ) : GameSystem
{
    public override void Execute()
    {
        changeLayout.IsDirty = messageQueue.Any();
        while (messageQueue.TryDequeue(out var message))
        {
            if (message.ChangeType == InventoryChangeType.Add)
            {
                inventory.IncrementCount(message.ItemType);
                if (message.ItemType == ItemType.SwordUpgrade)
                {
                    RecalculateDamage();
                }
            }
            else if (message.ChangeType == InventoryChangeType.Remove)
            {
                inventory.DecrementCount(message.ItemType);
            }
        }

        var toRemove = new List<ItemType>(0);
        foreach(var item in inventory.Items)
        {
            if (item.Count == 0)
            {
                toRemove.Add(item.ItemType);
            }
        }

        foreach(var toRemoveType in toRemove)
        {
            inventory.Items.RemoveAll(x => x.ItemType == toRemoveType);
        }
    }

    private void RecalculateDamage()
    {
        var damage = playerDamage.JoinComponent?.Item2;
        if (damage == null)
        {
            return;
        }

        var swords = inventory.Items.First(x => x.ItemType == ItemType.SwordUpgrade);

        playerDamage.T.Update(damage.Value.EntityId, new(1 + swords.Count));
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