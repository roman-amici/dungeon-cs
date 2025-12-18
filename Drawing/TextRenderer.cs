using System.Drawing;
using Ecs;
using SDL2;
using SdlAbstractions;

namespace Drawing;


public class TextRenderer(Screen screen, Font font)
{
    public Dictionary<TextDraw,TextSurface> Cache {get;} = new();

    public void DrawText(TextDraw draw, Point2D topLeft)
    {
        if (!Cache.TryGetValue(draw, out var surface))
        {
            surface = font.CreateTextureFromText(screen.Renderer, draw.Text, draw.FontSize, draw.Color.ToSdl());
        }

        var rect = new Rect2D()
        {
            TopLeft = topLeft,
            BottomRight = new Point2D(topLeft.X + surface.Texture.Width, topLeft.Y + surface.Texture.Height)
        };

        DrawTexture(surface.Texture, rect);
    }

    public void DrawTextCentered(TextDraw draw, Point2D center)
    {
        if (!Cache.TryGetValue(draw, out var surface))
        {
            surface = font.CreateTextureFromText(screen.Renderer, draw.Text, draw.FontSize, draw.Color.ToSdl());
        }

        var left = center.X - (surface.Texture.Width / 2);
        var top = center.Y - (surface.Texture.Height / 2);

        var rect = new Rect2D()
        {
            TopLeft = new Point2D(left,top),
            BottomRight = new Point2D(left + surface.Texture.Width, top + surface.Texture.Height)
        };

        DrawTexture(surface.Texture, rect);
    }

    private void DrawTexture(Texture texture, Rect2D destinationRect)
    {
        var sourceRect = new Rect2D
        {
            TopLeft = new Point2D(0,0),
            BottomRight = new Point2D(texture.Width, texture.Height)
        };
        screen.DrawTexture(texture, sourceRect, destinationRect);
        
    }
}

public struct TextDraw
{
    public TextDraw(string text, int fontSize, Color color)
    {
        Text = text;
        FontSize = fontSize;
        Color = color;
    }

    public string Text {get; set;}
    public int FontSize {get; set;} = 18;
    public Color Color {get; set;} = Color.White;

    public override int GetHashCode()
    {
        return (Text,FontSize,(Color.A,Color.R,Color.G,Color.B)).GetHashCode();
    }
}


