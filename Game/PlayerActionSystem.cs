using Ecs;

namespace Game;

public class PlayerActionSystem(
    Queue<PlayerAction> actions,
    Queue<WantsToMoveMessage> moves,
    SingletonJoin<Player, Health> playerHealth) : GameSystem
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
                case PlayerWait:
   
                    var health = healthComponent.Value;
                    health.AddHealth(1.0);
                    playerHealth.T.Update(healthComponent.EntityId, health);
                    
                    break;
            }
        }
    }
}