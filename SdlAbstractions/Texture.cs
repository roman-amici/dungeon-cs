using SDL2;

namespace SdlAbstractions;

public class Texture(nint texturePointer) : IDisposable
{
    public nint TexturePointer { get; } = texturePointer;

    public static unsafe Texture LoadTexture(nint renderer, string path)
    {
        var texture = SDL_image.IMG_LoadTexture(renderer, path);

        if (texture == IntPtr.Zero)
        {
            throw new Exception($"Failed to load texture {path}");
        }

        return new Texture(texture);
    }

    public void Dispose()
    {
        SDL.SDL_DestroyTexture(TexturePointer);
    }
}