using SDL2;
using SdlAbstractions;

namespace Drawing;

public class Screen(nint renderer)
{
    public void DrawTexture(Texture texture, Rect2D source, Rect2D destination)
    {
        var sourceBB = source.ToSdl();
        var destBB = destination.ToSdl();

        var result = SDL.SDL_RenderCopy(renderer, texture.TexturePointer, ref sourceBB, ref destBB);

        if (result != 0)
        {
            throw new Exception(SDL.SDL_GetError());
        }
    }
}