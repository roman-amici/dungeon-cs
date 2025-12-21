using Ecs;

namespace Game;

public class PickupItemSystem(
    World world,
    SingletonJoin<Player, Position> playerPosition,
    TableJoin<PickupItem, Position> itemPosition,
    PlayerInventory inventory,
    GameStateResource gameState) : GameSystem
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
            if (itemPosition.Value.MapPosition == player.MapPosition)
            {
                inventory.Items.Add(item.Value.ItemType);
                itemsToRemove.Add(item.EntityId);
            }
        }

        foreach(var entityId in itemsToRemove)
        {
            world.RemoveEntity(entityId);
        }

        if (inventory.Items.Contains(ItemType.Amulet))
        {
            gameState.CurrentState = GameState.PlayerWin;
        }
    }
}