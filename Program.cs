using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.TraceLogLevel;

using LeftEngine.Core;
using LeftEngine.World;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        // Setup
        SetTraceLogLevel(Warning | Error | Fatal);

        Config.Init();
        GameWindow.Init();
        AssetManager.Init();
        UI.Init();
        Camera.Init();

        // Testing
        var map = new Map(29, 29);
        var worldCenter = new Vector2(
            (map.Center.X - map.Center.Y) * 32,
            (map.Center.X + map.Center.Y) * 16
        );

        var playerPos = new Vector2(worldCenter.X - 32, worldCenter.Y - 16);
        Camera.Target = playerPos;

        // Main Loop
        while (!GameWindow.ShouldClose) {
            var DT = GetFrameTime();

            // Update
            Input.Update();
            GameWindow.Update(DT);
            Camera.Update(DT);
            UI.Update();

            if (Input.IsKeyPressed("escape")) { break; } // force close

            // Testing
            if (Input.IsKeyDown("w")) { playerPos.Y -= 3; }
            if (Input.IsKeyDown("s")) { playerPos.Y += 3; }
            if (Input.IsKeyDown("a")) { playerPos.X -= 3; }
            if (Input.IsKeyDown("d")) { playerPos.X += 3; }

            Camera.Target = playerPos;

            // Draw
            BeginDrawing();
                ClearBackground(Color.Black);

                BeginMode2D(Camera.Viewport);
                    map.Render();
                EndMode2D();

                UI.Draw();
            EndDrawing();
        }

        // Exit
        CloseWindow();
        UI.Exit();
    }
}
