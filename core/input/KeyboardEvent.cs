using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

enum InputDirection { Down, Up };

abstract class KeyEvent : Event {
    public KeyboardKey Key { get; private set; }

    public KeyEvent(KeyboardKey key, string name, EventType type, Action action) : base(type, action) {
        Key = key;
    }
}

class KeyPressEvent : KeyEvent {
    public KeyPressEvent(KeyboardKey key, EventType type, Action action) : base(key, $"KeyPress:{key}", type, action) {

    }
}

class KeyReleaseEvent : KeyEvent {
    public KeyReleaseEvent(KeyboardKey key, EventType type, Action action) : base(key, $"KeyRelease:{key}", type, action) {

    }
}

class KeyDownEvent : KeyEvent {
    public KeyDownEvent(KeyboardKey key, EventType type, Action action) : base(key, $"KeyDown:{key}", type, action) {

    }
}
