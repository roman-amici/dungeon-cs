using Map;

namespace Game;

public abstract class PlayerAction{}

public class PlayerMove(MapCoord destination) : PlayerAction
{
    public MapCoord Destination {get;} = destination;
}

public class PlayerWait : PlayerAction{}

public class UseItemAction(ItemType item) : PlayerAction
{
    public ItemType Item {get;} = item;
}