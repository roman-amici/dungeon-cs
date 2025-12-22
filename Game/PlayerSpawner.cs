using Drawing;
using Ecs;
using Map;

namespace Game;

public class PlayerSpawner(
    World world,
    DungeonMap<MapTile> map,
    Table<SpriteKey<SpriteTile>> sprites,
    Table<Position> positions,
    Singleton<Player> player,
    Table<Health> healths,
    Table<ToolTip> toolTip,
    Table<Damage> damages,
    Table<Collision> colliders
) : SpawningSystem<object?>(world)
{
    public override void Execute()
    {
        SpawnEntity(null);
    }

    protected override void Spawn(EntityId entityId, object? _)
    {
        player.Spawn(entityId, new());

        var tile = new SpriteKey<SpriteTile>(SpriteTile.Knight);

        sprites.Add(new Component<SpriteKey<SpriteTile>>(entityId, tile));

        var position = map.PlayerStart;

        if (map.Map[position.X,position.Y] != MapTile.Floor)
        {
            throw new InvalidOperationException("Player start is not on floor.");
        }

        positions.Add(new(entityId, new Position()
        {
            MapPosition = position
        }));

        healths.Add(entityId, new Health(20));
        toolTip.Add(entityId, new ToolTip("Player"));
        damages.Add(entityId, new Damage(1.0));
        colliders.Add(entityId, new Collision());
    }
}