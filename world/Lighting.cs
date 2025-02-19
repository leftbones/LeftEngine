using System.Numerics;

using LeftEngine.Core;
using LeftEngine.Utility;

namespace LeftEngine.World;

// Lighting System (WIP)
// - Lights are defined by a position, intensity, length, angle, and direction (see Light.cs for definitions)
// - The light map is a byte array that stores the light level of each cell
// - The ambient light level is the default light level of each cell
// - The light level of each cell is calculated based on the distance from each light source

// Known Issues:
// - None?

// Features to add:
// - High Priority:
//      - Light falloff at the edges of a light cone (currently the cones have hard edges)
//      - Softer shadows using a shader or a light texture maybe? Not sure how to project texture to isometric in 2D yet (currently shadows cover an entire tile)
// - Low Priority:
//      - Support for colored lights and color mixing (the light map would need to be a Color array instead of a byte array)
//      - "Realistic" shadows using a shader, like the method Chaos did but in glsl (shadows that are not blocky)


static class Lighting {
    public static readonly Vector2 North = new ( 0, -1);
    public static readonly Vector2 South = new ( 0,  1);
    public static readonly Vector2 East =  new ( 1,  0);
    public static readonly Vector2 West =  new (-1,  0);

    public static bool[] ComputeFOV(GridMap gridMap, Vector2 position, int length, int angle, Vector2 direction) {
        var visibleCells = new bool[gridMap.Width * gridMap.Height];

        var angleDirection = Math.Atan2(direction.Y, direction.X) * (180 / Math.PI);
        var circlePoints = Algorithms.GetCirclePoints(position, length);
        var arcPoints = angle == 360 ? circlePoints : Algorithms.TrimCircle(circlePoints, position, (int)angleDirection, angle); // Only trim the circle if the angle is less than 360

        if (angle == 1) {
            arcPoints = [position + direction * length];
        }

        for (int i = 0; i < arcPoints.Count; i++) {
            var linePoints = Algorithms.GetLinePoints(position, arcPoints[i]);

            for (int j = 0; j < linePoints.Count - 1; j++) {
                var point = linePoints[j];
                if (!gridMap.InBounds(point)) { continue; }

                visibleCells[(int)point.X + (int)point.Y * gridMap.Width] = true;

                var nextPoint = linePoints[j + 1];
                var castDir = Algorithms.GetDirection(point, nextPoint);

                if (gridMap.GetCell(point, out Cell? currCell)) {
                    if (castDir == North && currCell!.Tiles.Last().BlockVisionNS) { break; }
                    else if (castDir == West && currCell!.Tiles.Last().BlockVisionEW) { break; }
                }

                if (gridMap.GetCell(nextPoint, out Cell? nextCell)) {
                    if (nextCell != null) {
                        if (nextCell!.Tiles.Last().BlockVision) { visibleCells[(int)nextPoint.X + (int)nextPoint.Y * gridMap.Width] = true; break; }
                        if (castDir == South && nextCell!.Tiles.Last().BlockVisionNS) { break; }
                        else if (castDir == East && nextCell!.Tiles.Last().BlockVisionEW) { break; }
                    }
                }
            }
        }

        return visibleCells;
    }
}