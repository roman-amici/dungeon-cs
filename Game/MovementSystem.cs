using System.Runtime.InteropServices;
using Ecs;
using Map;

namespace Game;

public class MovementSystem(
    DungeonMap<MapTile> map,
    Queue<WantsToMoveMessage> moves,
    Queue<WantsToAttackMessage> attacks,
    Singleton<Player> player,
    Table<Position> positions,
    TableJoin<Collision,Position> colliders,
    DistanceMap distanceToPlayer,
    Table<Health> attacked) : GameSystem
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

            var occupied = colliders.FindComponentWhere((t) => t.Item2.MapPosition == move.NewPosition.MapPosition);
            if (occupied == null)
            {
                positions.Update(move.Entity, move.NewPosition);

                if (player.SingletComponent != null && player.SingletComponent.Value.EntityId == move.Entity)
                {
                    distanceToPlayer.UpdateFromMap(move.NewPosition.MapPosition, c => map.Map[c.X,c.Y] == MapTile.Floor);
                }
            }
            else
            {
                var (_,position) = occupied.Value;
                if (attacked.Find(position.EntityId) != null)
                {
                    attacks.Enqueue(new WantsToAttackMessage(move.Entity, occupied.Value.Item2.EntityId));
                }
            }
        }
    }
}

public struct WantsToMoveMessage(EntityId entity, Position newPosition)
{
    public EntityId Entity {get;} = entity;
    public Position NewPosition {get;} = newPosition;
}