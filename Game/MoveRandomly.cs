using System.Security.Cryptography;
using Ecs;
using Map;

namespace Game;

public struct MovingRandomly
{
    
}

public class MoveRandomlySystem(
    Queue<WantsToMoveMessage> moveQueue,
    TableJoin<MovingRandomly,Position> movers,
    Random rng) : GameSystem
{
    public override void Execute()
    {
        foreach( var (_,position) in movers.Components())
        {
            var coord = rng.Next(0,5) switch
            {
                0 => position.Value.MapPosition.SafeUp(),
                1 => position.Value.MapPosition.SafeLeft(),
                2 => position.Value.MapPosition.Down(),
                3 => position.Value.MapPosition.Right(),
                _ => null
            };

            if (coord != null)
            {
                moveQueue.Enqueue(new(position.EntityId, new(coord.Value)));
            }
        }
    }
}