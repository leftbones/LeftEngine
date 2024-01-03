using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        InitWindow(Global.WindowSize.X, Global.WindowSize.Y, "LeftEngine");
        SetTargetFPS(60);

        var Texture = LoadTexture("whitebase_stand_4dir.png");

        var Frames = new List<Frame>() {
            new(0, 0, 24, 32),
            new(24, 0, 24, 32),
            new(48, 0, 24, 32),
            new(72, 0, 24, 32)
        };

        var SpinAnimation = new Animation("Spin", Frames);

        var AnimationPlayer = new AnimationPlayer(new List<Animation>() { SpinAnimation });

        while (!WindowShouldClose()) {
            //
            // Update

            AnimationPlayer.Update();

            //
            // Draw
            BeginDrawing();
            ClearBackground(Color.DARKBLUE);

            AnimationPlayer.Draw(Texture);

            EndDrawing();
        }

        //
        // Exit
        CloseWindow();
    }
}
