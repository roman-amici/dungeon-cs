using Ecs;
using Map;

namespace Game;

public class ChasingPlayerSystem(
    DistanceMap playerDistance,
    TableJoin<ChasingPlayer, MapPosition> chasers,
    Table<MovingRandomly> randomMovers,
    Queue<WantsToMoveMessage> wantsToMove
) : GameSystem
{
    public override void Execute()
    {
        var toRemove = new List<EntityId>(0);
        foreach(var (_,position) in chasers.Components())
        {
            if (StartMovingRandomly(position.EntityId, position.Value.Coord))
            {
                toRemove.Add(position.EntityId);
                continue;
            }

            var nextCoord = playerDistance.NextNearest(position.Value.Coord);
            if (nextCoord != null)
            {
                wantsToMove.Enqueue(new(position.EntityId, new(nextCoord.Value)));
            }
        }

        foreach(var entityId in toRemove)
        {
            chasers.T1.Remove(entityId);
        }
    }

    private bool StartMovingRandomly(EntityId entityId, MapCoord coord)
    {
        if (playerDistance.Map[coord.X,coord.Y].Value >= 13)
        {
            randomMovers.Add(entityId, new());
            return true;
        }

        return false;
    }
}

public struct ChasingPlayer{}