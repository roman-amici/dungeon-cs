using Ecs;
using Input;
using Map;

namespace Game;

public class PlayerInputSystem(
    InputParser inputParser,
    SingletonJoin<Player,MapPosition> playerPosition,
    MouseLocation mouseLocation,
    Queue<PlayerAction> playerActions,
    UILayout layout,
    Turn turn
) : GameSystem
{
    public override void Execute()
    {
        turn.EndTurn = false;

        HandleMouseActions();
        HandleKeyboardActions();

        if (playerActions.Any())
        {
            turn.EndTurn = true;
        }
    }

    private void HandleMouseActions()
    {
        foreach (var mouseClickEvent in inputParser.MouseButtonEvents)
        {
            if (mouseClickEvent.isDown)
            {
                continue;
            }

            foreach(var uiObject in layout.UIObjects)
            {
                if (uiObject.Target == null || uiObject.Action == null)
                {
                    continue;
                }

                if (uiObject.Target.Value.Location.Contains(mouseClickEvent.Position))
                {
                    playerActions.Enqueue(uiObject.Action);
                }
            }
        }

        foreach (var mouseMoveEvent in inputParser.MouseMoveEvents)
        {
            mouseLocation.Point = mouseMoveEvent.Position;
        }

        if (mouseLocation.Point == null)
        {
            return;
        }
    }

    private void HandleKeyboardActions()
    {
        var p = playerPosition.Join;
        if (p == null)
        {
            return;
        }

        var (_,position) = p.Value;
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
                    newPosition = position.Coord.Down();
                    break;
                case Key.Up:
                    newPosition = position.Coord.Up();
                    break;
                case Key.Left:
                    newPosition = position.Coord.Left();
                    break;
                case Key.Right:
                    newPosition = position.Coord.Right();
                    break;
            }

            if (newPosition != null)
            {
                playerActions.Enqueue(new PlayerMove(newPosition.Value));
                break;
            }
        }
    }
}

public class MouseLocation
{
    public Point2D? Point {get; set;} = null;
}
