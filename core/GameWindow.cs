using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine.Core;

static class GameWindow {
    public static bool ShouldClose => WindowShouldClose();
    public static float FPS => GetFPS();

    public static Vector2 MousePos { get; private set; }

    public static void Init() {
        // SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);
        InitWindow((int)Config.Resolution.X, (int)Config.Resolution.Y, "LeftEngine");
        SetExitKey(KeyboardKey.Null);
        SetTargetFPS(60);
    }

    public static void Update(float delta) {
        MousePos = GetMousePosition();
    }
}