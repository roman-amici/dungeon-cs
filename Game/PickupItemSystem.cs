using Ecs;

namespace Game;

public class PickupItemSystem(
    SingletonJoin<Player, Position> playerPosition,
    TableJoin<PickupItem, Position> itemPosition,
    PlayerInventory inventory,
    GameStateResource gameState) : GameSystem
{
    public override void Execute()
    {
        if (!playerPosition.Any())
        {
            return;
        }

        var (_,player) = playerPosition.First();

        foreach(var (item,itemPosition) in itemPosition)
        {
            if (itemPosition.Value.MapPosition == player.Value.MapPosition)
            {
                inventory.Items.Add(item.Value.ItemType);
            }
        }

        if (inventory.Items.Contains(ItemType.Amulet))
        {
            gameState.CurrentState = GameState.PlayerWin;
        }
    }
}