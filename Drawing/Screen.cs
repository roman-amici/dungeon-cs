using System.Drawing;
using SDL2;
using SdlAbstractions;

namespace Drawing;

public class Screen(nint renderer)
{
    public nint Renderer {get;} = renderer;

    public void DrawTexture(Texture texture, Rect2D source, Rect2D destination)
    {
        var sourceBB = source.ToSdl();
        var destBB = destination.ToSdl();

        var result = SDL.SDL_RenderCopy(Renderer, texture.TexturePointer, ref sourceBB, ref destBB);

        if (result != 0)
        {
            throw new Exception(SDL.SDL_GetError());
        }
    }

    public void DrawRect(Rect2D rect, Color color)
    {
        if(SDL.SDL_SetRenderDrawColor(Renderer, color.R, color.G, color.B, color.A) < 0)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to set draw color: {err}");
        }

        var rectSDL = rect.ToSdl();
        if (SDL.SDL_RenderFillRect(Renderer, ref rectSDL) < 0)
        {
            var err = SDL.SDL_GetError();
            throw new Exception($"Failed to draw rect: {err}");
        }
    }
}