using Raylib_cs;
using static Raylib_cs.Raylib;
using LeftEngine.Core;
using LeftEngine.Core.Input;
using LeftEngine.Core.Screen;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        // Testing Stuff
        int CurrentColor = 0;
        List<Color> Colors = new() {
            Color.RED,
            Color.ORANGE,
            Color.YELLOW,
            Color.GREEN,
            Color.BLUE,
            Color.PURPLE
        };

        // Start the engine
        Engine.Start();

        // Set up global events
        Input.SetEvents(new() {
            { "Exit", new Event(EventType.Press, Engine.Exit) },
            { "ChangeBackground", new Event(EventType.Press, () => { CurrentColor = (CurrentColor + 1) % Colors.Count; Canvas.SetBackgroundColor(Colors[CurrentColor]); }) }
        });


        // Main loop
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
