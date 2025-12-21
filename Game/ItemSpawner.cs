using Drawing;
using Ecs;
using Map;

namespace Game;

public class ItemSpawner( 
    World world,
    DungeonMap<MapTile> map, 
    DistanceMap playerDistance, 
    SingletonJoin<Player,Position> playerPosition,
    Random rng,
    Table<Position> positions, 
    Table<PickupItem> items,
    Table<SpriteKey<SpriteTile>> sprites) : SpawningSystem<ItemSpawner.ItemSpawnerContext>(world)
{
    public override void Execute()
    {
        SpawnEntity(new ItemSpawnerContext(new(), ItemType.Amulet));

        foreach(var room in map.Rooms)
        {
            var i = rng.Next(0,12);

            if (i < 10)
            {
                continue;
            }

            if (i == 10)
            {
                SpawnEntity(new ItemSpawnerContext(room.Center, ItemType.SwordUpgrade));
            }
            else
            {
                SpawnEntity(new ItemSpawnerContext(room.Center, ItemType.Potion));
            }
        }
    }

    protected override void Spawn(EntityId entityId, ItemSpawnerContext context)
    {

        switch (context.ItemType)
        {
            case ItemType.Amulet:
                SpawnAmulet(entityId);
                break;
            default:
                SpawnItem(entityId, context);
                break;
        }

        sprites.Add(entityId, new(GetTile(context.ItemType)));
    }

    private void SpawnItem(EntityId entityId, ItemSpawnerContext context)
    {
        items.Add(entityId, new(context.ItemType));
        positions.Add(entityId, new(context.Location));
    }

    private void SpawnAmulet(EntityId entity)
    {
        var (_,position) = playerPosition.Join!.Value;

        if (playerDistance.IsDirty)
        {
            playerDistance.UpdateFromMap(position.MapPosition, (coord) => map.Map[coord.X,coord.Y] == MapTile.Floor);
        }

        uint maxDistance = 0;
        var maxPosition = position.MapPosition;
        for (var x = 0; x <  playerDistance.Map.GetLength(0); x++)
        {
            for (var y = 0; y < playerDistance.Map.GetLength(1); y++)
            {
                var distance = playerDistance.Map[x,y].Value;
                if ( distance == null)
                {
                    continue;
                }

                if ( distance.Value > maxDistance)
                {
                    maxDistance = distance.Value;
                    maxPosition = new MapCoord((uint)x,(uint)y);
                }
            }
        }

        items.Add(entity, new(ItemType.Amulet));
        positions.Add(entity, new(maxPosition));
    }

    private SpriteTile GetTile(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Amulet => SpriteTile.Amulet,
            ItemType.Potion => SpriteTile.Potion,
            ItemType.SwordUpgrade => SpriteTile.Sword,
            _ => SpriteTile.Potion
        };
    }

    public struct ItemSpawnerContext(MapCoord location, ItemType itemType)
    {
        public MapCoord Location {get;} = location;
        public ItemType ItemType {get;} = itemType;
    }
}