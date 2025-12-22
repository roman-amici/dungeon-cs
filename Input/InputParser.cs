using SDL2;

namespace Input;

public class InputParser
{
    public List<KeyboardEvent> KeyboardEvents {get;} = new();
    public List<MouseButtonEvent> MouseButtonEvents {get;} = new();
    public List<MouseMoveEvent> MouseMoveEvents {get;} = new();

    public void StartNewFrame()
    {
        KeyboardEvents.Clear();
        MouseButtonEvents.Clear();
        MouseMoveEvents.Clear();
    }

    public Key? MapKey (SDL.SDL_Keycode code)
    {
        return code switch
        {
            SDL.SDL_Keycode.SDLK_UP => Key.Up,
            SDL.SDL_Keycode.SDLK_DOWN => Key.Down,
            SDL.SDL_Keycode.SDLK_LEFT => Key.Left,
            SDL.SDL_Keycode.SDLK_RIGHT => Key.Right,
            SDL.SDL_Keycode.SDLK_SPACE => Key.Space,
            _ => null
        };

    }

    internal void HandleEvent(SDL.SDL_Event e)
    {
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_KEYDOWN:
                var keyDown = MapKey(e.key.keysym.sym);
                if (keyDown != null)
                {
                    KeyboardEvents.Add( new(keyDown.Value, true));
                }
                break;
            case SDL.SDL_EventType.SDL_KEYUP:
                var keyUp = MapKey(e.key.keysym.sym);
                if (keyUp != null)
                {
                    KeyboardEvents.Add( new(keyUp.Value, false));
                }
                break;
            case SDL.SDL_EventType.SDL_MOUSEMOTION:
                MouseMoveEvents.Add(new(e.motion.x,e.motion.y));
                break;
            case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                MouseButtonEvents.Add(new MouseButtonEvent(e.button.button, new(e.button.x, e.button.y), true));
                break;
            case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                MouseButtonEvents.Add(new MouseButtonEvent(e.button.button, new(e.button.x, e.button.y), false));
                break;
        }
    }
}