using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

enum Anchor { TopLeft, TopRight, BottomLeft, BottomRight, Center }

//
// Draw Methods
static partial class Canvas {
    private static Font DefaultFont { get; set; }           = GetFontDefault();
    private static int DefaultFontSize { get; set; }        = 16;
    private static int DefaultFontSpacing { get; set; }     = 0;
    private static Color DefaultTextColor { get; set; }     = Color.WHITE;

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
}
