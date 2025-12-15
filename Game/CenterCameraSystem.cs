using Drawing;
using Ecs;

namespace Game;

public class CenterCameraOnPlayerSystem(
    Camera camera,
    SingletonJoin<Player, Position> playerPosition) : GameSystem
{
    public override void Execute()
    {
        if (playerPosition.Any())
        {
            var (_,position) = playerPosition.First();

            camera.SetCenter(position.Value.MapPosition);
        }
    }
}