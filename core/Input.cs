using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.KeyboardKey;

namespace LeftEngine.Core;

static class Input {
    public static Vector2 MousePos => GetMousePosition();
    public static float MouseX => GetMouseX();
    public static float MouseY => GetMouseY();

    private static readonly Dictionary<string, int> keyMap = new() {
        // Mouse
        { "mouse_1", (int)MouseButton.Left},
        { "mouse_2", (int)MouseButton.Right},
        { "mouse_3", (int)MouseButton.Middle},
        { "mouse_wheel_up", 7 },
        { "mouse_wheel_down", 8 },

        // Modifiers
        { "l_shift", (int)LeftShift },
        { "r_shift", (int)RightShift },
        { "l_ctrl", (int)LeftControl },
        { "r_ctrl", (int)RightControl },
        { "l_alt", (int)LeftAlt },
        { "r_alt", (int)RightAlt },

        // Arrows
        { "up", (int)Up },
        { "down", (int)Down },
        { "left", (int)Left },
        { "right", (int)Right },

        // Special
        { "escape", (int)Escape },
        { "tab", (int)Tab },
        { "enter", (int)Enter },
        { "space", (int)Space },
        { "backspace", (int)Backspace },
        { "capslock", (int)CapsLock },
        { "delete", (int)Delete },
        { "home", (int)Home },
        { "end", (int)End },
        { "insert", (int)Insert },
        { "page_up", (int)PageUp },
        { "page_down", (int)PageDown },

        // Letters
        { "a", (int)A },
        { "b", (int)B },
        { "c", (int)C },
        { "d", (int)D },
        { "e", (int)E },
        { "f", (int)F },
        { "g", (int)G },
        { "h", (int)H },
        { "i", (int)I },
        { "j", (int)J },
        { "k", (int)K },
        { "l", (int)L },
        { "m", (int)M },
        { "n", (int)N },
        { "o", (int)O },
        { "p", (int)P },
        { "q", (int)Q },
        { "r", (int)R },
        { "s", (int)S },
        { "t", (int)T },
        { "u", (int)U },
        { "v", (int)V },
        { "w", (int)W },
        { "x", (int)X },
        { "y", (int)Y },
        { "z", (int)Z },

        // Numbers
        { "1", (int)One },
        { "2", (int)Two },
        { "3", (int)Three },
        { "4", (int)Four },
        { "5", (int)Five },
        { "6", (int)Six },
        { "7", (int)Seven },
        { "8", (int)Eight },
        { "9", (int)Nine },
        { "0", (int)Zero },

        // Function keys
        { "f1", (int)F1 },
        { "f2", (int)F2 },
        { "f3", (int)F3 },
        { "f4", (int)F4 },
        { "f5", (int)F5 },
        { "f6", (int)F6 },
        { "f7", (int)F7 },
        { "f8", (int)F8 },
        { "f9", (int)F9 },
        { "f10", (int)F10 },
        { "f11", (int)F11 },
        { "f12", (int)F12 },

        // Symbols
        { "-", (int)Minus },
        { "=", (int)Equal },
        { "[", (int)LeftBracket },
        { "]", (int)RightBracket },
        { ";", (int)Semicolon },
        { "'", (int)Apostrophe },
        { "`", (int)Grave },
        { "/", (int)Slash },
        { "\\", (int)Backslash },
        { ",", (int)Comma },
        { ".", (int)Period },

        // Keypad
        { "kp_1", (int)Kp1 },
        { "kp_2", (int)Kp2 },
        { "kp_3", (int)Kp3 },
        { "kp_4", (int)Kp4 },
        { "kp_5", (int)Kp5 },
        { "kp_6", (int)Kp6 },
        { "kp_7", (int)Kp7 },
        { "kp_8", (int)Kp8 },
        { "kp_9", (int)Kp9 },
        { "kp_0", (int)Kp0 },
        { "kp_enter", (int)KpEnter },
        { "kp_add", (int)KpAdd },
        { "kp_subtract", (int)KpSubtract },
        { "kp_multiply", (int)KpMultiply },
        { "kp_divide", (int)KpDivide },
        { "kp_decimal", (int)KpDecimal },
    };

    private static List<int> pressedKeys = [];
    private static List<int> releasedKeys = [];
    private static List<int> heldKeys = [];

    // Check if a key was pressed this frame
    public static bool IsKeyPressed(string key) {
        return pressedKeys.Contains(keyMap[key]);
    }

    // Check if a key was released this frame
    public static bool IsKeyReleased(string key) {
        return releasedKeys.Contains(keyMap[key]);
    }

    // Check if a key is being held
    public static bool IsKeyDown(string key) {
        return heldKeys.Contains(keyMap[key]);
    }

    // Read all input and update the key states
    public static void Update() {
        // Clear pressed and released keys queue
        pressedKeys.Clear();
        releasedKeys.Clear();

        // Check held keys and remove keys that have been released, adding them to the released keys queue
        for (int i = heldKeys.Count - 1; i >= 0; i--) {
            if (!Raylib.IsKeyDown((KeyboardKey)heldKeys[i])) {
                releasedKeys.Add(heldKeys[i]);
                heldKeys.RemoveAt(i);
            }
        }

        // Read input until there is none, adding each to the pressed keys queue
        var key = GetKeyPressed();
        while (key != 0) {
            pressedKeys.Add(key);
            key = GetKeyPressed();
        }

        // Add all keys pressed this frame to the held keys queue
        heldKeys.AddRange(pressedKeys);
    }
}