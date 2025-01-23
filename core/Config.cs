using System.Numerics;

namespace LeftEngine.Core;

static class Config {
    // Window Properties
    public static Vector2 Resolution = new(1280, 800);
    public static bool Fullscreen = false;
    public static bool UseSystemCursor = false;

    // Debug
    public static bool DebugEnabled = true;

    public static void Init() {

    }
}