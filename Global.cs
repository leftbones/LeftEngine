using Calcium;

namespace LeftEngine;

static class Global {
    // Build Info
    public const string BuildVer = "1.0.0-alpha";

    // Window
    public static Vector2i WindowSize = new(1280, 800);

    // Drawing
    public const int SpriteScale = 2; // All sprites are scaled by this number when drawn
}