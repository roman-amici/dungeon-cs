using Ecs;

namespace Game;

public class PlayerInventory
{
    public List<ItemEntry> Items {get;} = new();

    public bool TryIncrementCount(ItemType itemType)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemType == itemType)
            {
                var item = Items[i];
                item.Count += 1;
                Items[i] = item;

                return true;
            }
        }

        return false;
    }

    public bool TryDecrementCount(ItemType itemType)
    {
                for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemType == itemType)
            {
                var item = Items[i];
                item.Count -= 1;
                Items[i] = item;

                return true;
            }
        }

        return false;
    }
}

public struct ItemEntry(EntityId entity, ItemType itemType, int count)
{
    public EntityId Entity {get;} = entity;
    public ItemType ItemType {get;} = itemType;
    public int Count {get; set;} = count;
}