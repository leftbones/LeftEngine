using static Raylib_cs.Raylib;

namespace LeftEngine;

class Program {
    static void Main(string[] args) {
        Engine.Start();
        Canvas.SetDefaultFont("Kitchen Sink.ttf"); 

        // Entity Agnostic Events
        Input.SetEvents(new() {
            { "MoveUp", new Event(EventType.Hold, () => { Console.WriteLine("W"); }) },
            { "MoveDown", new Event(EventType.Hold, () => { Console.WriteLine("S"); }) },
            { "MoveLeft", new Event(EventType.Hold, () => { Console.WriteLine("A"); }) },
            { "MoveRight", new Event(EventType.Hold, () => { Console.WriteLine("D"); }) },
        });

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
