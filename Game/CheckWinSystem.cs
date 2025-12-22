using Ecs;

namespace Game;

public class CheckWinSystem(PlayerInventory inventory, GameStateResource gameState) : GameSystem
{
    public override void Execute()
    {
        if (inventory.Items.Any(x => x.ItemType == ItemType.Amulet))
        {
            gameState.CurrentState = GameState.PlayerWin;
        }
    }
}