using System.Numerics;
using Raylib_cs;

using LeftEngine.Utility;

namespace LeftEngine.Core;

static class Camera {
    public static Vector2 Position { get; private set; }
    public static Vector2 Target;
    public static Vector2 Offset;
    public static Camera2D Viewport;

    private static readonly float moveSpeed = 10.0f;

    public static void Init() {
        Position = Vector2.Zero;
        Target = Position;
        Offset = new Vector2(0, 40);

        Viewport = new Camera2D {
            Target = Position + Offset,
            Offset = new Vector2(Config.Resolution.X / 2, Config.Resolution.Y / 2),
            Rotation = 0.0f,
            Zoom = 1.0f
        };
    }

    public static void Update(float delta) {
        if (Position != Target) {
            Position = Vector2.Lerp(Position, Target, moveSpeed * delta);

            if (Vector2.Distance(Position, Target) < 0.1f) {
                Position = Target;
            }
        }

        Viewport.Target = Position + Offset;
    }

    public static void JumpToPos(Vector2 pos) {
        Position = pos;
        Target = pos;
    }

    public static Vector2 GetCellPos() {
        var tilePos = Algorithms.PointToCell(Position);
        return tilePos;
    }

    public static Vector2 GetCursorCellPos() {
        var mousePos = Input.MousePos + Position - Offset - Config.Resolution / 2;
        var tilePos = Algorithms.PointToCell(mousePos);
        return tilePos;
    }

    public static Vector2 GetCursorWorldPos() {
        var mousePos = Input.MousePos + Camera.Position + new Vector2(0, 80) - Camera.Offset - Config.Resolution / 2;
        return mousePos;
    }
}