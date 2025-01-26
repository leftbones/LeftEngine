using System.Numerics;

namespace LeftEngine.Utility;

static class Algorithms {
    public static List<List<Vector2>> GetConeLines(Vector2 start, Vector2 end, int radius) {
        List<List<Vector2>> lines = [];
        float angleStep = 1.0f; // Increase step for less points but less accuracy

        for (float angle = -radius / 2; angle <= radius / 2; angle += angleStep) {
            var points = new List<Vector2>();
            float radian = MathF.PI * angle / 180.0f;
            Vector2 direction = new(
                MathF.Cos(radian) * (end.X - start.X) - MathF.Sin(radian) * (end.Y - start.Y),
                MathF.Sin(radian) * (end.X - start.X) + MathF.Cos(radian) * (end.Y - start.Y)
            );
            Vector2 newEnd = start + direction;

            points.AddRange(GetLinePoints(start, newEnd));
            lines.Add(points);
        }

        return lines; // This must NOT return a distinct list, it has to return ALL points
    }

    public static List<Vector2> GetConePoints(Vector2 start, Vector2 end, int radius) {
        List<Vector2> points = [];
        float angleStep = 1.0f; // Increase step for less points but less accuracy

        for (float angle = -radius / 2; angle <= radius / 2; angle += angleStep) {
            float radian = MathF.PI * angle / 180.0f;
            Vector2 direction = new(
                MathF.Cos(radian) * (end.X - start.X) - MathF.Sin(radian) * (end.Y - start.Y),
                MathF.Sin(radian) * (end.X - start.X) + MathF.Cos(radian) * (end.Y - start.Y)
            );
            Vector2 newEnd = start + direction;
            points.AddRange(GetLinePoints(start, newEnd));
        }

        return points; // This must NOT return a distinct list, it has to return ALL points
    }

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