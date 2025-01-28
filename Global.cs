using System.Numerics;

namespace LeftEngine;

static class Global {
    public static Vector2 GridSize = new (32, 16);
    public static Vector2 TileSize = new (32, 112);
    public static Vector2 GridHalfSize = GridSize / 2;
    public static Vector2 TileHalfSize = TileSize / 2;
}