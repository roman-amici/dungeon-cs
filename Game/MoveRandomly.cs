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
    Table<ChasingPlayer> chasers,
    DistanceMap distanceMap,
    Random rng) : GameSystem
{
    public override void Execute()
    {
        var toRemove = new List<EntityId>();
        foreach( var (_,position) in movers.Components())
        {
            if (StartChasingPlayer(position.EntityId, position.Value.MapPosition))
            {
                toRemove.Add(position.EntityId);
                continue;
            }
            
            MapCoord? coord = rng.Next(0,5) switch
            {
                0 => position.Value.MapPosition.Up(),
                1 => position.Value.MapPosition.Left(),
                2 => position.Value.MapPosition.Down(),
                3 => position.Value.MapPosition.Right(),
                _ => null
            };

            if (coord != null)
            {
                moveQueue.Enqueue(new(position.EntityId, new(coord.Value)));
            }
        }

        foreach(var entityId in toRemove)
        {
            movers.T1.Remove(entityId);
        }
    }

    private bool StartChasingPlayer(EntityId entityId, MapCoord currentPosition)
    {   
        var distance = distanceMap.Map[currentPosition.X, currentPosition.Y];
        if (distance.Value <= 10)
        {
            chasers.Add(entityId, new());

            return true;
        }

        return false;
    }
}