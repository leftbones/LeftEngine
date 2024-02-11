using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        Engine.Start();
        Canvas.SetDefaultFont("Kitchen Sink.ttf"); 

        while (!WindowShouldClose()) {
            Engine.Update();
            Engine.Draw();
        }

        //
        // Exit
        Engine.Exit();
        CloseWindow();
    }
}
