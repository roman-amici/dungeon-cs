using SDL2;

namespace SdlAbstractions;

public class TextSurface : IDisposable
{
    private TextSurface(nint renderer, nint surfacePointer)
    {
        SurfacePointer = surfacePointer;

        var texture = SDL.SDL_CreateTextureFromSurface(renderer, SurfacePointer);
        if (texture == IntPtr.Zero)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to create texture from surface: {err}");
        }

        Texture = new Texture(texture);
    }

    public static TextSurface RenderText(nint renderer, Font font, string text, int ptSize, SDL.SDL_Color color)
    {
        var result = SDL_ttf.TTF_SetFontSize(font.FontPointer, ptSize);
        if (result < 0)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to set font size: {err}");
        }
        
        var surface = SDL_ttf.TTF_RenderText_Blended(font.FontPointer, text, color);
        if (surface == IntPtr.Zero)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to create text surface: {err}");
        }

        try
        {
            return new TextSurface(renderer, surface);
        }
        catch
        {
            SDL.SDL_FreeSurface(surface);
            throw;
        }
    }

    public nint SurfacePointer {get;}

    public Texture Texture {get;}

    public void Dispose()
    {
        SDL.SDL_FreeSurface(SurfacePointer);
        Texture.Dispose();
    }
}