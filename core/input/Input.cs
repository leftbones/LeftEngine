using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

static class Input {
    private static readonly List<Key> InputStream = new();
    private static readonly List<int> HeldKeys = new();

    private static readonly Dictionary<int, List<Event>> KeyBinds = new();

    private static Dictionary<string, Event> EventMap;
    // private static readonly Dictionary<string, Event> EventMap = new() {
    //     { "MoveUp", new Event(EventType.Hold, () => { Console.WriteLine("W"); }) },
    //     { "MoveDown", new Event(EventType.Hold, () => { Console.WriteLine("S"); }) },
    //     { "MoveLeft", new Event(EventType.Hold, () => { Console.WriteLine("A"); }) },
    //     { "MoveRight", new Event(EventType.Hold, () => { Console.WriteLine("D"); }) },
    // };

    public static void Update() {
        // Clear all events from the previous update
        InputStream.Clear();

        // Read input until there are no more events
        var Input = (int)GetKeyPressed();
        while (Input != 0) {
            var Key = new Key(EventType.Press, Input);
            InputStream.Add(Key);

            if (!HeldKeys.Contains(Input)) {
                HeldKeys.Add(Input);
            }

            Input = GetKeyPressed();
        }

        // Check the list of held keys and remove those which are no longer held
        for (int i = HeldKeys.Count - 1; i >= 0; i--) {
            if (!IsKeyDown((KeyboardKey)HeldKeys[i])) {
                var Key = new Key(EventType.Release, HeldKeys[i]);
                InputStream.Add(Key);
                HeldKeys.Remove(HeldKeys[i]);
            }
        }

        // Handle held keys
        foreach (var Code in HeldKeys) {
            var Key = new Key(EventType.Hold, Code);
            InputStream.Add(Key);
        }

        // Process all keys in the InputStream, fire events if possible
        foreach (var Key in InputStream) {
            var Handled = false;

            foreach (var Token in ControlSystem.Tokens) {
                if (Token.FireEvent(Key)) {
                    Handled = true;
                    continue;
                }
            }

            if (!Handled && KeyBinds.TryGetValue(Key.Code, out List<Event> EventList)) {
                foreach (var Event in EventList) {
                    if (Event.Type == Key.Type || Event.Type == EventType.Any) {
                        Event.Fire();
                    }
                }
            }
        }
    }

    public static void SetEvents(Dictionary<string, Event> map) {
        EventMap = map;
        ApplyKeymap();
    }

    public static void ApplyKeymap() {
        foreach (var Item in Config.Keymap) {
            if (Global.InputMap.TryGetValue(Item.Key, out int value)) {
                var EventList = new List<Event>();
                foreach (var Event in Item.Value) {
                    EventList.Add(EventMap[Event]);
                }

                KeyBinds.Add(value, EventList);
            }
        }
    }
}
