using Ecs;
using Input;
using Map;

namespace Game;

public class PlayerInputSystem(
    InputParser inputParser,
    SingletonJoin<Player,Position> playerPosition,
    MouseLocation mouseLocation,
    Queue<PlayerAction> playerActions,
    Turn turn
) : GameSystem
{
    public override void Execute()
    {
        turn.EndTurn = false;

        foreach (var mouseMoveEvent in inputParser.MouseMoveEvents)
        {
            mouseLocation.Point = mouseMoveEvent.Position;
        }

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

            if (keyEvent.Key == Key.Space)
            {
                playerActions.Enqueue(new PlayerWait());
                break;
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
                playerActions.Enqueue(new PlayerMove(newPosition.Value));
                break;
            }
        }

        if (playerActions.Any())
        {
            turn.EndTurn = true;
        }
    }
}

public class MouseLocation
{
    public Point2D? Point {get; set;} = null;
}
