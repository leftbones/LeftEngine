using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

static class Input {
    private static readonly List<Key> InputStream = new List<Key>();
    private static readonly List<int> HeldKeys = new List<int>();

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
        for (int i = HeldKeys.Count() - 1; i >= 0; i--) {
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
            Console.WriteLine(Key);
        }
    }
}
