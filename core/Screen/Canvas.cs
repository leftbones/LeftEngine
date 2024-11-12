using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine.Core.Screen;

enum Anchor { TopLeft, TopRight, BottomLeft, BottomRight, Center }

//
// Draw Methods
static partial class Canvas {
    private static Font DefaultFont { get; set; }           = LoadFontEx("Assets/Kitchen Sink.ttf", 96, null, 256);
    private static int DefaultFontSize { get; set; }        = 16;
    private static int DefaultFontSpacing { get; set; }     = 0;
    private static Color DefaultTextColor { get; set; }     = Color.WHITE;

    public static Color BackgroundColor { get; set; }      = Color.DARKBLUE;


    // Draw text to the screen
    public static void DrawText(string text, int x, int y, int? font_size=null, int? font_spacing=null, Color? color=null, Anchor anchor=Anchor.TopLeft) {
        var FontSize = font_size ?? DefaultFontSize;
        var FontSpacing = font_spacing ?? DefaultFontSpacing;
        var TextColor = color ?? DefaultTextColor;

        var AnchorPos = new Vector2(x, y);
        switch (anchor) {
            case Anchor.TopLeft:
                break;

            case Anchor.TopRight:
                AnchorPos -= new Vector2(MeasureTextEx(DefaultFont, text, FontSize, FontSpacing).X, 0);
                break;

            case Anchor.BottomLeft:
                AnchorPos -= new Vector2(0, MeasureTextEx(DefaultFont, text, FontSize, FontSpacing).Y);
                break;

            case Anchor.BottomRight:
                AnchorPos -= MeasureTextEx(DefaultFont, text, FontSize, FontSpacing);
                break;

            case Anchor.Center:
                AnchorPos -= MeasureTextEx(DefaultFont, text, FontSize, FontSpacing) / 2;
                break;
        }

        DrawTextEx(DefaultFont, text, AnchorPos, FontSize, FontSpacing, TextColor);
    }


    // Helper function for drawing text with a drop shadow (by calling DrawText twice)
    public static void DrawTextShadow(string text, int x, int y, int offset=2, int? font_size=null, int? font_spacing=null, Color? color=null, Anchor anchor=Anchor.TopLeft) {
        DrawText(text, x + offset, y + offset, font_size, font_spacing, Color.BLACK, anchor);
        DrawText(text, x, y, font_size, font_spacing, color, anchor);
    }
}

//
// Property Modifiers
static partial class Canvas {
    public static void SetDefaultFont(string font_path, int max_size=96, int char_count=256) {
        DefaultFont = LoadFontEx(font_path, max_size, null, char_count);
    }

    public static void SetDefaultFontSize(int size) {
        DefaultFontSize = size;
    }

    public static void SetDefaultFontSpacing(int spacing) {
        DefaultFontSpacing = spacing;
    }

    public static void SetDefaultTextColor(Color color) {
        DefaultTextColor = color;
    }

    public static void SetBackgroundColor(Color color) {
        BackgroundColor = color;
    }
}
