using SDL2;

namespace SdlAbstractions;

public class Font : IDisposable
{
    private Font(nint fontPointer)
    {
        FontPointer = fontPointer;
    }

    public nint FontPointer {get;}

    public static Font LoadFont(string fontPath)
    {
        var fontPtr = SDL_ttf.TTF_OpenFont(fontPath, 24);
        if (fontPtr == IntPtr.Zero)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to load font! : {err}");
        }

        return new Font(fontPtr);
    }

    public TextSurface CreateTextureFromText(nint renderer, string text, int ptSize, SDL.SDL_Color color)
    {
        return TextSurface.RenderText(renderer, this, text, ptSize, color);
    }

    public void Dispose()
    {
        SDL_ttf.TTF_CloseFont(FontPointer);
    }
}