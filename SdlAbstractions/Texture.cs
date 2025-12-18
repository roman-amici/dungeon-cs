using SDL2;

namespace SdlAbstractions;

public class Texture : IDisposable
{
    public Texture(nint texturePointer)
    {
        TexturePointer = texturePointer;
        var result = SDL.SDL_QueryTexture(TexturePointer, out _, out _, out var width, out var height);
        if (result < 0)
        {
            var error = SDL.SDL_GetError();
            throw new Exception($"Texture query failed: {error}");
        }

        Width = width;
        Height = height;
    }

    public nint TexturePointer { get; }
    public int Width {get;}
    public int Height {get;}

    public static Texture LoadTexture(nint renderer, string path)
    {
        var texture = SDL_image.IMG_LoadTexture(renderer, path);

        if (texture == IntPtr.Zero)
        {
            var message = SDL.SDL_GetError();
            throw new Exception($"Failed to load texture: {message}");
        }

        return new Texture(texture);
    }

    public void Dispose()
    {
        SDL.SDL_DestroyTexture(TexturePointer);
    }
}