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
    private bool[] visionMap;

    private readonly List<Light> lights = [];

    private Vector2 playerPos;

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
                SetTile(cx, cy, 0, new Tile("white_tile") { IsFloor = true });
            }
        }

        // Test (add walls to the north and west sides of the map)
        for (int i = 0; i < width; i++) {
            gridMap.AddTile(i, 0, new Tile("wall_stone_n") { IsWall = true, BlockVisionNS = true });
            gridMap.AddTile(0, i, new Tile("wall_stone_w") { IsWall = true, BlockVisionEW = true });
        }

        // Test (add a western wall to the white tile area)
        for (int i = 0; i < 7; i++) {
            gridMap.AddTile((int)Center.X - 3, (int)Center.Y - 3 + i, new Tile("wall_stone_w") { IsWall = true, BlockVisionEW = true });
        }

        // Test (Add some random crates)
        for (int i = 0; i < 10; i++) {
            var cx = RNG.Range(0, width);
            var cy = RNG.Range(0, height);
            if (gridMap.GetCell(cx, cy, out Cell? cell)) { if (cell!.Tiles.Last().ID == "white_tile") { continue; } }
            gridMap.AddTile(cx, cy, new Tile("crate_wood") { Height = 29, BlockVisionNS = true, BlockVisionEW = true, BlockMovementNS = true, BlockMovementEW = true });
        }

        // Add some test lights
        lights.Add(new Light(new Vector2(0, 0), 150, 11));
        lights.Add(new Light(new Vector2(Width - 1, Height - 1), 150, 11));
        lights.Add(new Light(new Vector2(0, Height - 1), 150, 11));
        lights.Add(new Light(new Vector2(Width - 1, 0), 150, 11));

        // Add the player pos
        playerPos = Center;

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

        // Update the "player" to point towards the cursor and move with the camera
        playerPos = Camera.GetCellPos();
        var playerDirection = Camera.GetCursorCellPos() - playerPos;
        visionMap = Lighting.ComputeFOV(gridMap, playerPos, 20, 150, playerDirection);

        // Update the light map for each light source
        foreach (var light in lights) {
            var visibleCells = Lighting.ComputeFOV(gridMap, light.Position, light.Length, light.Angle, light.Direction);

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    if (!visibleCells[x + y * Width]) { continue; }
                    var point = new Vector2(x, y);

                    float distance = Vector2.Distance(point, light.Position);
                    byte currentLightLevel = lightMap[(int)point.X + (int)point.Y * gridMap.Width];
                    byte newLightLevel = (byte)Math.Clamp(currentLightLevel + light.Intensity * (1 - (distance / light.Length)), ambientLightLevel, 255);

                    lightMap[(int)point.X + (int)point.Y * gridMap.Width] = newLightLevel;
                }
            }
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
                    var tileVisible = visionMap[x + y * Width];
                    var lightLevel = tileVisible ? lightMap[x + y * Width] / 255.0f : (ambientLightLevel - 10) / 255.0f;
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
        foreach (var light in lights) {
            var iconTexture = AssetManager.Icons["lightbulb"];
            var lightPos = Algorithms.CellToPoint(light.Position) - new Vector2(Global.TileSize.X / 2, iconTexture.Height); 
            DrawTextureV(iconTexture, lightPos, light.Color);
        }
    }
}