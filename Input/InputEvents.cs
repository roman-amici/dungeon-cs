using System.Drawing;

namespace Input;

public enum Key
{
    Up,
    Down,
    Left,
    Right,
    Space,
}

public struct KeyboardEvent(Key key, bool isDown)
{
    public Key Key {get;} = key;
    public bool IsDown {get;} = isDown;
}

public struct MouseButtonEvent(int button, Point2D position, bool isDown)
{
    public int Button {get;} = button;
    public bool isDown {get;} = isDown;
    public Point2D Position {get;} = position;
}

public struct MouseMoveEvent(int x, int y)
{
    public Point2D Position {get;} = new(x,y);
}