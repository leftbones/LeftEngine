using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        InitWindow(Global.WindowSize.X, Global.WindowSize.Y, "LeftEngine");
        SetTargetFPS(60);

        Canvas.SetDefaultFont("Kitchen Sink.ttf");

        var TextureWalkUp = LoadTexture("whitebase_walk_up.png");
        var TextureWalkDown = LoadTexture("whitebase_walk_down.png");
        var TextureWalkLeft = LoadTexture("whitebase_walk_left.png");
        var TextureWalkRight = LoadTexture("whitebase_walk_right.png");

        var AP = new AnimationPlayer(
            new List<Animation>() {
               new("walk_up", TextureWalkUp, 0, 0, 24, 32, 4),
               new("walk_down", TextureWalkDown, 0, 0, 24, 32, 4),
               new("walk_left", TextureWalkLeft, 0, 0, 24, 32, 4),
               new("walk_right", TextureWalkRight, 0, 0, 24, 32, 4),
            }
        );

        AP.Animations["walk_up"].OnAnimationFinished = () => { AP.Play("walk_right"); };
        AP.Animations["walk_right"].OnAnimationFinished = () => { AP.Play("walk_down"); };
        AP.Animations["walk_down"].OnAnimationFinished = () => { AP.Play("walk_left"); };
        AP.Animations["walk_left"].OnAnimationFinished = () => { AP.Play("walk_up"); };

        while (!WindowShouldClose()) {
            //
            // Update

            AP.Update();

            //
            // Draw
            BeginDrawing();
            ClearBackground(Color.DARKBLUE);

            AP.Draw();

            Canvas.DrawText($"LeftEngine {Global.BuildVer}", 8, Global.WindowSize.Y - 8, 12, anchor: Anchor.BottomLeft);

            EndDrawing();
        }

        //
        // Exit
        CloseWindow();
    }
}
