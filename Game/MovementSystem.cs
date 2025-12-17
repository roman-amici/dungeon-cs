using System.Runtime.InteropServices;
using Ecs;
using Map;

namespace Game;

public class MovementSystem(
    DungeonMap<MapTile> map,
    Queue<WantsToMoveMessage> moves,
    Queue<WantsToAttackMessage> attacks,
    Table<Position> positions) : GameSystem
{
    public override void Execute()
    {
        while(moves.TryDequeue(out var move))
        {
            var tile = map.SafeGet(move.NewPosition.MapPosition);
            if (tile == null)
            {
                continue;
            }

            if (tile != MapTile.Floor)
            {
                continue;
            }

            var occupied = positions.FirstWhere(x => x.Value.MapPosition == move.NewPosition.MapPosition);
            if (occupied == null)
            {
                positions.Update(move.Entity, move.NewPosition);
            }
            else
            {
                attacks.Enqueue(new WantsToAttackMessage(move.Entity, occupied.Value.EntityId));
            }
        }
    }
}

public struct WantsToMoveMessage(EntityId entity, Position newPosition)
{
    public EntityId Entity {get;} = entity;
    public Position NewPosition {get;} = newPosition;
}