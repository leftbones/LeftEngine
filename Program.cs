using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.TraceLogLevel;

using LeftEngine.Core;
using LeftEngine.World;
using LeftEngine.Utility;

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

        var playerPos = new Vector2(worldCenter.X + 32, worldCenter.Y + 16);
        Camera.JumpToPos(playerPos);

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
            if (Input.IsKeyDown("w")) { playerPos.Y -= 1.5f; }
            if (Input.IsKeyDown("s")) { playerPos.Y += 1.5f; }
            if (Input.IsKeyDown("a")) { playerPos.X -= 3.0f; }
            if (Input.IsKeyDown("d")) { playerPos.X += 3.0f; }

            Camera.Target = playerPos;

            // Draw
            BeginDrawing();
                ClearBackground(Color.Black);

                BeginMode2D(Camera.Viewport);
                    map.Render();
                    var col = new Color(255, 255, 255, 125);
                    DrawCircleV(Camera.Position, 8.0f, col); // Player "head"
                    DrawCircleV(Camera.Position + new Vector2(0, 40), 4.0f, col); // Player "center" (camera target)
                    DrawEllipse((int)Camera.Position.X, (int)Camera.Position.Y + 80, 22.0f, 11.0f, col); // Player "feet"

                    // Player cell position (absolute)
                    var camPos = Algorithms.CellToPoint(Camera.GetCellPos());
                    DrawEllipse((int)camPos.X, (int)camPos.Y, 16.0f, 8.0f, col);

                    // Mouse position in world
                    // var mousePos = Camera.GetCursorWorldPos();
                    // DrawEllipse((int)mousePos.X, (int)mousePos.Y, 16.0f, 8.0f, Color.Red);
                EndMode2D();

                UI.Draw();
            EndDrawing();
        }

        // Exit
        CloseWindow();
        UI.Exit();
    }
}
