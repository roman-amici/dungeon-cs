using Ecs;
using Map;

namespace Game;

public class MovementSystem(
    DungeonMap<MapTile> map,
    Queue<WantsToMoveMessage> moves,
    Table<Position> positions) : GameSystem
{
    public override void Execute()
    {
        foreach(var move in moves)
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

            positions.Update(move.Entity, move.NewPosition);
        }
        
        moves.Clear();
    }
}

public struct WantsToMoveMessage(EntityId entity, Position newPosition)
{
    public EntityId Entity {get;} = entity;
    public Position NewPosition {get;} = newPosition;
}