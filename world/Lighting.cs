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
// - Certain cells look like they should be occluded but are not (may not be a bug)
// - Sometimes a wall will be shadowed even though it shouldn't be due to the wall next to it "blocking" it
// - A light source that is placed in a corner is blocked incorrectly

// Features to add:
// - Support for colored lights and color mixing (the light map would need to be a Color array instead of a byte array)
// - Support for light sources that flicker or move
// - Light falloff at the edges of a light cone (currently the cones have hard edges)
// - Softer shadows using a shader or a light texture maybe? Not sure how to project texture to isometric in 2D yet (currently shadows cover an entire tile)
// - "Realistic" shadows using a shader, like the method Chaos did but in glsl (shadows that are not blocky)


// Current issue:
// There's "holes" in the light at certain angles causing shadows to appear on walls where they shouldn't
// Unsure if using Array.Copy is slower than using a point cache, but it's less code

static class Lighting {
    public static readonly Vector2 North = new ( 0, -1);
    public static readonly Vector2 South = new ( 0,  1);
    public static readonly Vector2 East =  new ( 1,  0);
    public static readonly Vector2 West =  new (-1,  0);

    public static void ComputeLight(Light light, byte[] lightMap, GridMap gridMap, byte ambientLightLevel) {
        var lightMapChanges = new byte[lightMap.Length];
        lightMap.CopyTo(lightMapChanges, 0);

        var angleDirection = Math.Atan2(light.Direction.Y, light.Direction.X) * (180 / Math.PI);
        var circlePoints = Algorithms.GetCirclePoints(light.Position, light.Length);
        var arcPoints = light.Angle == 360 ? circlePoints : Algorithms.TrimCircle(circlePoints, light.Position, (int)angleDirection, light.Angle); // Only trim the circle if the angle is less than 360

        if (light.Angle == 1) {
            arcPoints = [light.Position + light.Direction * light.Length];
        }

        for (int i = 0; i < arcPoints.Count; i++) {
            var linePoints = Algorithms.GetLinePoints(light.Position, arcPoints[i]);

            for (int j = 0; j < linePoints.Count - 1; j++) {
                var point = linePoints[j];
                if (!gridMap.InBounds(point)) { continue; } // is this a problem?

                float distance = Vector2.Distance(point, light.Position);
                byte currentLightLevel = lightMap[(int)point.X + (int)point.Y * gridMap.Width];
                byte newLightLevel = (byte)Math.Clamp(currentLightLevel + light.Intensity * (1 - (distance / light.Length)), ambientLightLevel, 255);

                lightMapChanges[(int)point.X + (int)point.Y * gridMap.Width] = newLightLevel;

                if (gridMap.GetCell(point, out Cell? cell)) {
                    var lastTile = cell!.Tiles.Last();
                    var direction = Algorithms.GetDirection(point, linePoints[j + 1]);

                    if (lastTile.BlockVision) { break; }

                    if (lastTile.BlockVisionNS) {
                        if (direction == North) { break; }
                    }

                    if (lastTile.BlockVisionEW) {
                        if (direction == West) { break; }
                    }
                }
            }
        }

        lightMapChanges.CopyTo(lightMap, 0);
    }
}