using System.Numerics;

namespace LeftEngine.World;

static class FOV {
    public static byte[] Compute(Vector2 origin, byte[] lightMap, GridMap gridMap) {
        lightMap[(int)origin.X + (int)origin.Y * gridMap.Width] = 255;
        return lightMap;
    }
}