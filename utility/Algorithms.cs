using System.Numerics;

namespace LeftEngine.Utility;

static class Algorithms {
    // Translate a point on screen to a cell position
    public static Vector2 PointToCell(Vector2 point) {
        var pos = new Vector2(
            (float)Math.Round((point.X / 32 + point.Y / 16) / 2) - 1,
            (float)Math.Round((point.Y / 16 - point.X / 32) / 2)
        );
        return pos;
    }

    // Translate a cell position to a point on screen
    public static Vector2 CellToPoint(Vector2 cell) {
        var pos = new Vector2(
            ((cell.X - cell.Y) * 32) + 32,
            ((cell.X + cell.Y) * 16) + 96
        );
        return pos;
    }

    // Get the points of an arc of a given radius in the direction of a start to end point
    public static List<Vector2> GetArcPoints(Vector2 start, Vector2 end, int radius) {
        List<Vector2> points = [];
        float angleStep = 1.0f; // Increase step for less points but less accuracy

        for (float angle = -radius / 2; angle <= radius / 2; angle += angleStep) {
            float radian = MathF.PI * angle / 180.0f;
            Vector2 direction = new(
                MathF.Cos(radian) * (end.X - start.X) - MathF.Sin(radian) * (end.Y - start.Y),
                MathF.Sin(radian) * (end.X - start.X) + MathF.Cos(radian) * (end.Y - start.Y)
            );
            points.Add(start + direction);
        }

        return points;
    }

    // Get the points of a line between two points
    public static List<Vector2> GetLinePoints(Vector2 start, Vector2 end) {
        var points = new List<Vector2>();

        int x0 = (int)start.X;
        int y0 = (int)start.Y;
        int x1 = (int)end.X;
        int y1 = (int)end.Y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true) {
            points.Add(new Vector2(x0, y0));

            if (x0 == x1 && y0 == y1) { break; }
            int e2 = 2 * err;

            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            } else if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }

        return points;
    }
}