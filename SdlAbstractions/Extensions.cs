using System.Drawing;
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

    public static SDL.SDL_Color ToSdl(this Color color)
    {
        return new SDL.SDL_Color()
        {
            a = color.A,
            r = color.R,
            g = color.G,
            b = color.B
        };
    }
}