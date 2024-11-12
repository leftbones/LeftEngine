using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine.Core.Input;

static class Input {
    private static readonly List<Key> InputStream = new();                      // Stores all keys pressed during the current update
    private static readonly List<int> HeldKeys = new();                         // Stores all keys currently held down until they are released

    private static readonly Dictionary<int, List<Event>> KeyBinds = new();      // Stores all key bindings and their associated events

    private static Dictionary<string, Event> EventMap;                          // Stores all events available to be bound to keys


    // Clear the input stream, read all input events, then process them
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

            if (!Handled && KeyBinds.TryGetValue(Key.Code, out List<Event> EventList)) {
                foreach (var Event in EventList) {
                    if (Event.Type == Key.Type || Event.Type == EventType.Any) {
                        Event.Fire();
                    }
                }
            }
        }
    }

    // Populate the EventMap dictionary with the events given
    public static void SetEvents(Dictionary<string, Event> map) {
        EventMap = map;
        ApplyKeymap();
    }

    // Apply the keymap from the config file to the KeyBinds dictionary
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
