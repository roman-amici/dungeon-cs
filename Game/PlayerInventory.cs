using Ecs;

namespace Game;

public class PlayerInventory
{
    public List<ItemEntry> Items {get;} = new();

    public void IncrementCount(ItemType itemType)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemType == itemType)
            {
                var item = Items[i];
                item.Count += 1;
                Items[i] = item;

                return;
            }
        }

        Items.Add(new(itemType, 1));
    }

    public void DecrementCount(ItemType itemType)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemType == itemType)
            {
                var item = Items[i];
                item.Count -= 1;
                Items[i] = item;

                return;
            }
        }
    }
}

public struct ItemEntry(ItemType itemType, int count)
{
    public ItemType ItemType {get;} = itemType;
    public int Count {get; set;} = count;
}