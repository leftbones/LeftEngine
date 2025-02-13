using System.Numerics;

namespace LeftEngine.Utility;

static class Algorithms {
    // Get the cardinal direction from one point to another
    public static Vector2 GetDirection(Vector2 start, Vector2 end) {
        Vector2 delta = end - start;
        if (Math.Abs(delta.X) > Math.Abs(delta.Y)) {
            return delta.X > 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
        } else {
            return delta.Y > 0 ? new Vector2(0, 1) : new Vector2(0, -1);
        }
    }

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

    public static List<Vector2> GetCirclePoints(Vector2 origin, int radius) {
        List<Vector2> points = [];

        int x = 0;
        int y = radius;
        int d = 3 - 2 * radius;

        points.Add(new Vector2(origin.X + radius, origin.Y));
        points.Add(new Vector2(origin.X - radius, origin.Y));
        points.Add(new Vector2(origin.X, origin.Y + radius));
        points.Add(new Vector2(origin.X, origin.Y - radius));

        while (y >= x) {
            if (d > 0) {
                y--;
                d = d + 4 * (x - y) + 10;
            } else {
                d = d + 4 * x + 6;
            }
            x++;

            points.Add(new Vector2(origin.X + x, origin.Y + y));
            points.Add(new Vector2(origin.X - x, origin.Y + y));
            points.Add(new Vector2(origin.X + x, origin.Y - y));
            points.Add(new Vector2(origin.X - x, origin.Y - y));
            points.Add(new Vector2(origin.X + y, origin.Y + x));
            points.Add(new Vector2(origin.X - y, origin.Y + x));
            points.Add(new Vector2(origin.X + y, origin.Y - x));
            points.Add(new Vector2(origin.X - y, origin.Y - x));
        }

        return points;
    }

    public static List<Vector2> TrimCircle(List<Vector2> points, Vector2 origin, int angle, int size) {
        List<Vector2> newPoints = [];
        int angleStart = (angle - size / 2 + 360) % 360;
        int angleEnd = (angle + size / 2 + 360) % 360;

        foreach (var point in points) {
            var a = Math.Atan2(point.Y - origin.Y, point.X - origin.X) * (180 / Math.PI);
            if (a < 0) {
                a += 360;
            }

            if (angleStart <= angleEnd) {
                if (a >= angleStart && a <= angleEnd) {
                    newPoints.Add(point);
                }
            } else {
                if (a >= angleStart || a <= angleEnd) {
                    newPoints.Add(point);
                }
            }
        }

        return newPoints;
    }

    // Get the points of a RASTERIZED line between two points (cells are connected in cardinal directions)
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