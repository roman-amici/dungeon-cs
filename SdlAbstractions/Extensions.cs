using SDL2;

namespace SdlAbstractions;

public static class Extensions
{
    public static SDL.SDL_Rect ToSdl(this Rect2D rect)
    {
        return new SDL.SDL_Rect()
        {
            x = (int)rect.TopLeft.X,
            y = (int)rect.TopLeft.Y,
            w = (int)Math.Abs(rect.TopLeft.X - rect.BottomRight.X),
            h = (int)Math.Abs(rect.TopLeft.Y - rect.BottomRight.Y),
        };
    }
}