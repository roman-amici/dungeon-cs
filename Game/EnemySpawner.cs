using Drawing;
using Ecs;
using Map;

namespace Game;

public class EnemySpawner(
    DungeonMap<MapTile> map,
    Table<Position> positions,
    Table<SpriteKey<SpriteTile>> sprites,
    Table<Enemy> enemies,
    Table<ToolTip> tooltips,
    Table<Health> healths,
    Table<Damage> damages,
    Table<MovingRandomly> randomMovers,
    Random rng) : Spawner
{
    public void SpawnEnemies(World world)
    {
        foreach(var room in map.Rooms)
        {
            var position = room.Center;

            world.SpawnEntity(this, new EnemySpawnerContext
            {
                 SpawnPosition = position
            });

        }
    }

    public override void Spawn(EntityId entityId, object? context)
    {
        if (context is not EnemySpawnerContext spawnerContext)
        {
            throw new InvalidOperationException("Incorrect type");
        }

        enemies.Add(entityId, new());

        var tile = rng.Next(0,12) switch
        {
          <= 6 => SpriteTile.Goblin,
          <= 9 => SpriteTile.Orc,
          10 => SpriteTile.Entin,
          11 => SpriteTile.Ogre,
          _ => throw new IndexOutOfRangeException()
        };
        tile = SpriteTile.Entin;

        positions.Add(entityId, new (spawnerContext.SpawnPosition));
        sprites.Add(entityId, new SpriteKey<SpriteTile>(tile));
        tooltips.Add(entityId, new ToolTip(GetName(tile)));
        healths.Add(entityId, new Health(GetHealth(tile)));
        randomMovers.Add(entityId, new MovingRandomly());
        damages.Add(entityId, new Damage(GetDamage(tile)));
    }

    public string GetName(SpriteTile tile)
    {
        return tile.ToString();
    }

    public double GetHealth(SpriteTile tile)
    {
        return tile switch
        {
            SpriteTile.Knight => 20,
            SpriteTile.Goblin => 1,
            SpriteTile.Orc => 2,
            SpriteTile.Entin => 3,
            SpriteTile.Ogre => 4,
            _ => throw new IndexOutOfRangeException(),
        };
    }

    public double GetDamage(SpriteTile tile)
    {
        return tile switch
        {
            SpriteTile.Knight => 1.0,
            SpriteTile.Goblin => 1.0,
            SpriteTile.Orc => 1.5,
            SpriteTile.Entin => 2,
            SpriteTile.Ogre => 2.5,
            _ => throw new IndexOutOfRangeException(),
        };
    }
}

public class EnemySpawnerContext
{
    public required MapCoord SpawnPosition {get; set;}
}