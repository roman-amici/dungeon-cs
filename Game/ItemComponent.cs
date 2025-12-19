namespace Game;

public struct PickupItem(ItemType itemType)
{
    public ItemType ItemType {get;} = itemType;
}

public enum ItemType
{
    Amulet,
    Potion,
    SwordUpgrade
}