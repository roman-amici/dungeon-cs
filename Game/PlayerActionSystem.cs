using Ecs;

namespace Game;

public class PlayerActionSystem(
    Queue<PlayerAction> actions,
    Queue<WantsToMoveMessage> moves,
    SingletonJoin<Player, Health> playerHealth,
    Queue<InventoryChangeMessage> inventoryChange,
    PlayerInventory inventory) : GameSystem
{
    public override void Execute()
    {
        var j = playerHealth.JoinComponent;
        if (j == null)
        {
            return;
        }

        var ( _,healthComponent) = j.Value;

        while(actions.TryDequeue(out var action))
        {
            switch (action)
            {
                case PlayerMove move:
                    moves.Enqueue(new(healthComponent.EntityId, new(move.Destination)));
                    break;
                case UseItemAction useItem:
                    TryUseItem(useItem, healthComponent);
                    break;
            }
        }
    }

    private void TryUseItem(UseItemAction useItem, Component<Health> health)
    {
        var index = inventory.Items.FindIndex(x => x.ItemType == useItem.Item);
        if (index == -1)
        {
            return;
        }

        var itemEntry = inventory.Items[index];
        if (itemEntry.Count == 0)
        {
            return;
        }

        switch (itemEntry.ItemType)
        {
            case ItemType.Potion:
                var newHealth = health.Value;
                newHealth.AddHealth(health.Value.MaxHealth);
                playerHealth.T.Update(health.EntityId, health.Value);
                inventoryChange.Enqueue(new(InventoryChangeType.Remove, ItemType.Potion));
                break;
        }
    }
}