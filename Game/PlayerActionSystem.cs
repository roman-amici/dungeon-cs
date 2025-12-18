using Ecs;

namespace Game;

public class PlayerActionSystem(
    Queue<PlayerAction> actions,
    Queue<WantsToMoveMessage> moves,
    SingletonJoin<Player, Health> playerHealth) : GameSystem
{
    public override void Execute()
    {
        if (playerHealth.Single.First == null)
        {
            return;
        }

        while(actions.TryDequeue(out var action))
        {
            switch (action)
            {
                case PlayerMove move:
                    moves.Enqueue(new(playerHealth.Single.First.Value.EntityId, new(move.Destination)));
                    break;
                case PlayerWait:
                    if (playerHealth.Any())
                    {
                        var (_,health) = playerHealth.First();
                        health.Value.AddHealth(1.0);
                        playerHealth.Table.Update(health.EntityId, health.Value);
                    }
                    break;
            }
        }
    }
}