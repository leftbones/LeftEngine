using System.Numerics;
using Calcium;
using Raylib_cs;
using static Raylib_cs.Raylib;

using LeftEngine.Core;
using LeftEngine.Utility;

namespace LeftEngine.World;

class Map {
    public Vector2 Size { get; private set; }
    public Vector2 Center { get; private set; }

    public int Width => (int)Size.X;
    public int Height => (int)Size.Y;

    private readonly byte ambientLightLevel = 150;

    private readonly GridMap gridMap;
    private readonly byte[] lightMap;
    private readonly bool[] visionMap;

    private readonly List<Light> lights = [];

    public Map(int width, int height) {
        Size = new Vector2(width, height);
        Center = new Vector2(width / 2, height / 2);

        // Set up the tile map
        gridMap = new GridMap(width, height);

        // Build the light map
        lightMap = new byte[width * height];
        Array.Fill(lightMap, ambientLightLevel);

        // Build the vision map
        visionMap = new bool[width * height];
        Array.Fill(visionMap, false);

        // ========================================================================================================
        // Testing
        // ========================================================================================================

        // Test (create a center area of white tiles)
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                var cx = (int)Center.X - 3 + x;
                var cy = (int)Center.Y - 3 + y;
                SetTile(cx, cy, 0, new Tile("white_tile"));
            }
        }

        // Test (add walls to the north and west sides of the map)
        for (int i = 0; i < width; i++) {
            gridMap.AddTile(i, 0, new Tile("wall_stone_n", tags: TileTags.BlockVisionNS));
            gridMap.AddTile(0, i, new Tile("wall_stone_w", tags: TileTags.BlockVisionEW));
        }

        // Test (Add some random crates)
        // for (int i = 0; i < 25; i++) {
        //     var cx = RNG.Range(0, width);
        //     var cy = RNG.Range(0, height);
        //     if (gridMap.GetCell(cx, cy, out Cell? cell)) { if (cell!.Tiles.Last().ID == "white_tile") { continue; } }
        //     var tile = new Tile("crate_wood", 29);
        //     gridMap.AddTile(cx, cy, new Tile("crate_wood", 29, tags: TileTags.BlockVision | TileTags.BlockMovement));
        // }

        // Add some test lights
        lights.Add(new Light(Center, 255, 20, 150)); // "Player" FOV
        lights.Add(new Light(new Vector2(0, 0), 100, 8));
        lights.Add(new Light(new Vector2(Width - 1, Height - 1), 100, 8));
        lights.Add(new Light(new Vector2(0, Height - 1), 100, 8));
        lights.Add(new Light(new Vector2(Width - 1, 0), 100, 8));

        // ========================================================================================================
    }

    // Set a tile at a given position
    public bool SetTile(Vector2 pos, int layer, Tile tile)  { return gridMap.SetTile((int)pos.X, (int)pos.Y, layer, tile); }
    public bool SetTile(int x, int y, int layer, Tile tile) {
        return gridMap.SetTile(x, y, layer, tile);
    }

    // Get at tile at a given position
    public bool GetTile(Vector2 pos, int layer, out Tile? tile)  { return gridMap.GetTile((int)pos.X, (int)pos.Y, layer, out tile); }
    public bool GetTile(int x, int y, int layer, out Tile? tile) {
        return gridMap.GetTile(x, y, layer, out tile);
    }

    // Get a cell at a given position
    public bool GetCell(Vector2 pos, out Cell? cell)  { return gridMap.GetCell((int)pos.X, (int)pos.Y, out cell); }
    public bool GetCell(int x, int y, out Cell? cell) {
        return gridMap.GetCell(x, y, out cell);
    }

    // Mark a cell as visible
    public void MarkVisible(Vector2 pos)  { MarkVisible((int)pos.X, (int)pos.Y); }
    public void MarkVisible(int x, int y) {
        visionMap[x + y * Width] = true;
    }

    // Update the lightMap array for all lights
    public unsafe void UpdateLightmap() {
        Array.Fill(lightMap, ambientLightLevel);

        // Update the test light to point towards the cursor and move with the camera
        var testLight = lights[0];
        testLight.Position = Camera.GetCellPos();
        testLight.Direction = Camera.GetCursorCellPos() - testLight.Position;

        // Update the light map for each light source
        foreach (var light in lights) {
            Lighting.ComputeLight(light, lightMap, gridMap, ambientLightLevel);
        }
    }

    // Render all tiles in the grid map
    public void Render() {
        UpdateLightmap();

        // Render the tile map
        var cursorCellPos = Camera.GetCursorCellPos();
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                var currentCellPos = new Vector2(x, y);

                if (gridMap.GetCell(x, y, out Cell? cell)) {
                    var lightLevel = lightMap[x + y * Width] / 255.0f;
                    var tileColor = new Color(lightLevel, lightLevel, lightLevel);
                    var heightOffset = 0;

                    foreach (var tile in cell!.Tiles) {
                        var texture = AssetManager.Textures[tile.ID];
                        var tileX = (x - y) * (int)Global.GridSize.X;
                        var tileY = (x + y) * (int)Global.GridSize.Y;

                        DrawTexture(texture, tileX, tileY - heightOffset, tileColor);
                        heightOffset += tile.Height;

                        if (currentCellPos == cursorCellPos && heightOffset == 0) {
                            var cursorPoint = Algorithms.CellToPoint(cursorCellPos);
                            cursorPoint -= new Vector2(32, 96);
                            DrawTextureV(AssetManager.Textures["cursor"], cursorPoint + new Vector2(0, 2), Color.Black);
                            DrawTextureV(AssetManager.Textures["cursor"], cursorPoint, Color.White);
                        }
                    }
                }
            }
        }

        // Render debug icons
        for (int i = 1; i < lights.Count; i++) {
            var light = lights[i];
            var iconTexture = AssetManager.Icons["lightbulb"];
            var lightPos = Algorithms.CellToPoint(light.Position) - new Vector2(Global.TileSize.X / 2, iconTexture.Height); 
            DrawTextureV(iconTexture, lightPos, light.Color);
        }

        // Lighting Testing
        Vector2 North = new ( 0, -1);
        Vector2 South = new ( 0,  1);
        Vector2 East =  new ( 1,  0);
        Vector2 West =  new (-1,  0);
        foreach (var light in lights) {
            var angleDirection = Math.Atan2(light.Direction.Y, light.Direction.X) * (180 / Math.PI);
            var circlePoints = Algorithms.GetCirclePoints(light.Position, light.Length);
            var arcPoints = light.Angle == 360 ? circlePoints : Algorithms.TrimCircle(circlePoints, light.Position, (int)angleDirection, light.Angle); // Only trim the circle if the angle is less than 360

            var pointCache = new List<Vector2>();
            for (int i = 0; i < arcPoints.Count - 1; i++) {
                var testPoints = new List<Vector2>();
                var drawTestLine = false;

                var linePoints = Algorithms.GetLinePoints(light.Position, arcPoints[i]);
                foreach (var point in linePoints) {
                    if (gridMap.GetCell(point, out Cell? cell)) {
                        testPoints.Add(point);

                        if (!pointCache.Contains(point)) {
                            pointCache.Add(point);
                        }

                        if (cell!.Tiles.Last().Tags.HasFlag(TileTags.BlockVision)) { drawTestLine = true; break; } // Tile blocks vision from all directions

                        var nextPoint = i + 1 < linePoints.Count ? linePoints[i + 1] : linePoints.Last();
                        var castDirection = Algorithms.GetDirection(point, nextPoint);

                        var dirPoint = point + castDirection;

                        if (castDirection == North || castDirection == South) {
                            if (cell.Tiles.Last().Tags.HasFlag(TileTags.BlockVisionNS)) { testPoints.Add(dirPoint); drawTestLine = true; break; }
                        }

                        if (castDirection == East || castDirection == West) {
                            if (cell.Tiles.Last().Tags.HasFlag(TileTags.BlockVisionEW)) { testPoints.Add(dirPoint); drawTestLine = true; break; }
                        }
                    } else { drawTestLine = true; break; }
                }

                if (drawTestLine) {
                    for (int j = 1; j < testPoints.Count; j++) {
                        var lineStart = Algorithms.CellToPoint(testPoints[j - 1]);
                        var lineEnd = Algorithms.CellToPoint(testPoints[j]);
                        DrawLineV(lineStart, lineEnd, Color.White);
                    }

                    var lastPoint = Algorithms.CellToPoint(testPoints.Last());
                    DrawCircleV(lastPoint, 4, Color.White);
                }
            }
        }
    }
}