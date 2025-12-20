using Drawing;
using Ecs;

namespace Game;

public class CenterCameraOnPlayerSystem(
    Camera camera,
    SingletonJoin<Player, Position> playerPosition) : GameSystem
{
    public override void Execute()
    {
        var pPosition = playerPosition.Join;
        if (pPosition != null)
        {
            var (_,position) = pPosition.Value;

            camera.SetCenter(position.MapPosition);
        }
    }
}