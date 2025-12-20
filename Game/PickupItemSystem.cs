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
        var pPosition = playerPosition.Join;
        if (pPosition == null)
        {
            return;
        }

        var (_,player) = pPosition.Value;

        foreach(var (item,itemPosition) in itemPosition)
        {
            if (itemPosition.MapPosition == player.MapPosition)
            {
                inventory.Items.Add(item.ItemType);
            }
        }

        if (inventory.Items.Contains(ItemType.Amulet))
        {
            gameState.CurrentState = GameState.PlayerWin;
        }
    }
}