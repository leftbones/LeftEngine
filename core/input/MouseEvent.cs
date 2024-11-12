using Calcium;
using Raylib_cs;

namespace LeftEngine.Core.Input;

abstract class MouseEvent : Event {
    public MouseButton? Button { get; private set; }
    
    public MouseEvent(MouseButton? button, string name, EventType type, Action action) : base(type, action) {
        Button = button;
    }
}

class MousePressEvent : MouseEvent {
    public Vector2i Position { get; private set; }

    public MousePressEvent(MouseButton button, Vector2i position, EventType type, Action action) : base(button, $"MousePress:{button}", type, action) {
        Position = position;
    }
}

class MouseReleaseEvent : MouseEvent {
    public Vector2i Position { get; private set; }

    public MouseReleaseEvent(MouseButton button, Vector2i position, EventType type, Action action) : base(button, $"MouseRelease:{button}", type, action) {
        Position = position;
    }
}

class MouseDownEvent : MouseEvent {
    public Vector2i Position { get; private set; }

    public MouseDownEvent(MouseButton button, Vector2i position, EventType type, Action action) : base(button, $"MouseDown:{button}", type, action) {
        Position = position;
    }
}

class MouseWheelEvent : MouseEvent {
    public int Amount { get; private set; }

    public MouseWheelEvent(MouseButton button, int amount, EventType type, Action action) : base(null, $"MouseWheel:{button}", type, action) {
        Amount = amount;
    }
}