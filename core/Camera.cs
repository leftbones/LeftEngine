using System.Numerics;
using Raylib_cs;

namespace LeftEngine.Core;

static class Camera {
    public static Vector2 Position { get; private set; }
    public static Vector2 Target;
    public static Camera2D Viewport;

    private static readonly float moveSpeed = 10.0f;

    public static void Init() {
        Position = Vector2.Zero;
        Target = Position;

        Viewport = new Camera2D {
            Target = Position,
            Offset = new Vector2(Config.Resolution.X / 2, Config.Resolution.Y / 2),
            Rotation = 0.0f,
            Zoom = 1.0f
        };
    }

    public static void Update(float delta) {
        if (Position != Target) {
            Position = Vector2.Lerp(Position, Target, moveSpeed * delta);
        }

        Viewport.Target = Position;
    }

    public static Vector2 GetTilePos() {
        var tilePos = new Vector2(
            (float)Math.Round((Target.X / 32 + Target.Y / 16) / 2) - 1,
            (float)Math.Round((Target.Y / 16 - Target.X / 32) / 2)
        );
        return tilePos;
    }

    public static Vector2 GetCursorTilePos() {
        var mousePos = Input.MousePos + Position - Config.Resolution / 2;
        var tilePos = new Vector2(
            (float)Math.Round((mousePos.X / 32 + mousePos.Y / 16) / 2) - 1,
            (float)Math.Round((mousePos.Y / 16 - mousePos.X / 32) / 2)
        );
        return tilePos;
    }
}