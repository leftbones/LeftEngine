using Calcium;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;


//
// Core Methods
static partial class Engine {
    public static void Start() {
        // Load and apply the keymap
        Config.LoadKeymap();

        // Create window
        InitWindow(Config.WindowSize.X, Config.WindowSize.Y, Global.WindowTitle);
        SetWindowPosition(1400, 540); // Linux tries to put the window in between both of my monitors for some reason, this moves it over to my main monitor

        SetTargetFPS(60);

        Console.WriteLine("Engine started.");
    }

    public static void Update() {
        Input.Update();
    }

    public static void Draw() {
        BeginDrawing();
        ClearBackground(Color.DARKBLUE);

        Canvas.DrawText($"LeftEngine {Global.BuildVer}", 8, Config.WindowSize.Y - 8, 16, anchor: Anchor.BottomLeft);

        EndDrawing(); 
    }

    public static void Exit() {
        Console.WriteLine("Engine stopped."); }
}


//
// Properties
static partial class Engine {
    public static float DeltaTime => GetFrameTime();
}

//
// Property Modifiers
static partial class Engine {
    public static void SetWindowSize(int width, int height) {
        Config.WindowSize = new Vector2i(width, height);
        SetWindowSize(width, height);
    }

    public static void ToggleFullscreen() {
        Config.Fullscreen = !Config.Fullscreen;
        ToggleFullscreen();
    }
}