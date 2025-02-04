using System.Numerics;

using LeftEngine.Core;
using LeftEngine.Utility;

namespace LeftEngine.World;

static class Lighting {
    public static void ComputeLight(Light light, byte[] lightMap, GridMap gridMap, byte ambientLightLevel) {
        var cursorPos = Camera.GetCursorCellPos(); var circlePoints = Algorithms.GetCirclePoints(light.Position, light.Distance);
        var cursorAngle = Math.Atan2(cursorPos.Y - light.Position.Y, cursorPos.X - light.Position.X) * (180 / Math.PI);
        var arcPoints = Algorithms.TrimCircle(circlePoints, light.Position, (int)cursorAngle, 150);
        var pointCache = new List<Vector2>();

        for (int i = 0; i < arcPoints.Count - 1; i++) {
            var linePoints = Algorithms.GetLinePoints(light.Position, arcPoints[i]);

            foreach (var point in linePoints) {
                if (gridMap.GetCell(point, out Cell? cell)) {
                    if (!pointCache.Contains(point)) {
                        float distance = Vector2.Distance(point, light.Position);
                        byte currentLightLevel = lightMap[(int)point.X + (int)point.Y * gridMap.Width];
                        byte newLightLevel = (byte)Math.Clamp(currentLightLevel + light.Intensity * (1 - (distance / light.Distance)), ambientLightLevel, 255);
                        lightMap[(int)point.X + (int)point.Y * gridMap.Width] = newLightLevel;
                        pointCache.Add(point);
                    }

                    if (cell!.Tiles.Last().BlockVision) { break; }
                }
            }
        }
    }
}