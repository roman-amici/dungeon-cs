using Ecs;
using Input;
using Map;

namespace Game;

public class PlayerInputSystem(
    InputParser inputParser,
    SingletonJoin<Player,Position> playerPosition,
    Queue<WantsToMoveMessage> moves
) : GameSystem
{
    public override void Execute()
    {
        if (!playerPosition.Any())
        {
            return;
        }

        var (_,position) = playerPosition.First();
        foreach(var keyEvent in inputParser.KeyboardEvents)
        {
            if (!keyEvent.IsDown)
            {
                continue;
            }

            MapCoord? newPosition = null;;
            switch (keyEvent.Key)
            {
                case Key.Down:
                    newPosition = position.Value.MapPosition.Down();
                    break;
                case Key.Up:
                    newPosition = position.Value.MapPosition.SafeUp();
                    break;
                case Key.Left:
                    newPosition = position.Value.MapPosition.SafeLeft();
                    break;
                case Key.Right:
                    newPosition = position.Value.MapPosition.Right();
                    break;
            }

            if (newPosition != null)
            {
                moves.Enqueue(new WantsToMoveMessage(position.EntityId, new(newPosition.Value)));
            }
        }
    }
}