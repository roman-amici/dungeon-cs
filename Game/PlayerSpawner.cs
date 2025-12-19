using Drawing;
using Ecs;
using Map;

namespace Game;

public class PlayerSpawner(
    DungeonMap<MapTile> map,
    Table<SpriteKey<SpriteTile>> sprites,
    Table<Position> positions,
    Singleton<Player> player,
    Table<Health> healths,
    Table<ToolTip> toolTip,
    Table<Damage> damages,
    Table<Collision> colliders
) : Spawner
{
    public override void Spawn(EntityId entityId, object? _)
    {
        player.Spawn(entityId, new());

        var tile = new SpriteKey<SpriteTile>(SpriteTile.Knight);

        sprites.Add(new Component<SpriteKey<SpriteTile>>(entityId, tile));

        MapCoord? position = null;
        for (var i = 0; i < map.Map.GetLength(0); i++)
        {
            for (var j = 0; j < map.Map.GetLength(1); j++)
            {
                if (map.Map[i, j] == MapTile.Floor)
                {
                    position = new MapCoord((uint)i, (uint)j);
                    break;
                }
            }
        }

        if (position == null)
        {
            throw new InvalidOperationException("Map has no floor");
        }

        positions.Add(new(entityId, new Position()
        {
            MapPosition = position.Value
        }));

        healths.Add(entityId, new Health(20));
        toolTip.Add(entityId, new ToolTip("Player"));
        damages.Add(entityId, new Damage(1.0));
        colliders.Add(entityId, new Collision());

    }
}