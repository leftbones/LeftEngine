using Calcium;
using Raylib_cs;
using static Raylib_cs.Raylib;
using LeftEngine.Core.Screen;
using LeftEngine.Core.Input;

namespace LeftEngine.Core;


//
// Core Methods
static partial class Engine {
    // Start the engine and its components
    public static void Start() {
        // Load and apply the keymap
        Config.LoadKeymap();

        // Create window
        InitWindow(Config.WindowSize.X, Config.WindowSize.Y, Global.WindowTitle);
        SetExitKey(KeyboardKey.KEY_NULL);
        SetTargetFPS(60);

        Console.WriteLine("Engine started");
    }

    // Update all engine components
    public static void Update() {
        Input.Update();
    }

    // Draw all engine components
    public static void Draw() {
        BeginDrawing();
        ClearBackground(Canvas.BackgroundColor);

        Canvas.DrawTextShadow($"LeftEngine {Global.BuildVer}", 8, Config.WindowSize.Y - 8,  anchor: Anchor.BottomLeft);

        EndDrawing();
    }

    // Shut down the engine
    public static void Exit() {
        Console.WriteLine("Engine stopped");
    }
}


//
// Properties
static partial class Engine {
    public static float DeltaTime => GetFrameTime();
}


//
// Property Modifiers
static partial class Engine {
    // Set the current window size
    public static void SetWindowSize(int width, int height) {
        Config.WindowSize = new Vector2i(width, height);
        SetWindowSize(width, height);
    }

    // Toggle fullscreen mode
    public static void ToggleFullscreen() {
        Config.Fullscreen = !Config.Fullscreen;
        ToggleFullscreen();
    }
}