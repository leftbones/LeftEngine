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
// - Softer shadows using a shader (currently shadows cover an entire tile)
// - "Realistic" shadows using a shader (shadows that are not blocky)

// FIX FOR NEXT TIME SO I DON'T FORGET:
// the tile that the light is ON should not be able to block the light from moving to adjacent tiles
// basically when checking a point, check the point that the light is moving to, not the point that the light is on
// if the point that the light is moving to is blocked, that tile should be illuminated, then the light should stop

static class Lighting {
    public static readonly Vector2 North = new ( 0, -1);
    public static readonly Vector2 South = new ( 0,  1);
    public static readonly Vector2 East =  new ( 1,  0);
    public static readonly Vector2 West =  new (-1,  0);

    public static void ComputeLight(Light light, byte[] lightMap, GridMap gridMap, byte ambientLightLevel) {
        var angleDirection = Math.Atan2(light.Direction.Y, light.Direction.X) * (180 / Math.PI);
        var circlePoints = Algorithms.GetCirclePoints(light.Position, light.Length);
        var arcPoints = light.Angle == 360 ? circlePoints : Algorithms.TrimCircle(circlePoints, light.Position, (int)angleDirection, light.Angle); // Only trim the circle if the angle is less than 360

        var pointCache = new List<Vector2>();
        for (int i = 0; i < arcPoints.Count - 1; i++) {
            var linePoints = Algorithms.GetLinePoints(light.Position, arcPoints[i]);

            foreach (var point in linePoints) {
                if (gridMap.GetCell(point, out Cell? cell)) {
                    if (!pointCache.Contains(point)) {
                        float distance = Vector2.Distance(point, light.Position);
                        byte currentLightLevel = lightMap[(int)point.X + (int)point.Y * gridMap.Width];
                        byte newLightLevel = (byte)Math.Clamp(currentLightLevel + light.Intensity * (1 - (distance / light.Length)), ambientLightLevel, 255);

                        lightMap[(int)point.X + (int)point.Y * gridMap.Width] = newLightLevel;

                        pointCache.Add(point);
                    }

                    if (cell!.Tiles.Last().Tags.HasFlag(TileTags.BlockVision)) { break; } // Tile blocks vision from all directions

                    var nextPoint = i + 1 < linePoints.Count ? linePoints[i + 1] : linePoints.Last();
                    var castDirection = Algorithms.GetDirection(point, nextPoint);

                    if (castDirection == North || castDirection == South) {
                        if (cell.Tiles.Last().Tags.HasFlag(TileTags.BlockVisionNS)) { break; }
                    }

                    if (castDirection == East || castDirection == West) {
                        if (cell.Tiles.Last().Tags.HasFlag(TileTags.BlockVisionEW)) { break; }
                    }
                } else { break; }
            }
        }
    }
}