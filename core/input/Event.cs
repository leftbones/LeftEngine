namespace LeftEngine.Core.Input;

public enum EventType { Press, Release, Hold, Any };

class Event {
    public EventType Type { get; private set; }
    public Action Action { get; private set; }

    public Event(EventType type, Action action) {
        Type = type;
        Action = action;
    }

    public void Fire() {
        Action?.Invoke();
    }
}

class Key {
    public EventType Type { get; private set; }
    public int Code { get; private set; }

    public Key(EventType type, int code) {
        Type = type;
        Code = code;
    }
}
