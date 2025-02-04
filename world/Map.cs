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

    private readonly List<Light> lights = [];

    public Map(int width, int height) {
        Size = new Vector2(width, height);
        Center = new Vector2(width / 2, height / 2);

        // Set up the tile map
        gridMap = new GridMap(width, height);

        // Test (create a center area of white tiles)
        for (int x = 0; x < 7; x++) {
            for (int y = 0; y < 7; y++) {
                var cx = (int)Center.X - 3 + x;
                var cy = (int)Center.Y - 3 + y;
                SetTile(cx, cy, 0, new Tile("white_tile"));
            }
        }

        // Test (set the north and west edge tiles to walls)
        for (int i = 0; i < width; i++) {
            gridMap.AddTile(i, 0, new Tile("wall_stone_n", blockVision: true));
            gridMap.AddTile(0, i, new Tile("wall_stone_w", blockVision: true));
        }

        // Test (Add some random crates)
        for (int i = 0; i < 20; i++) {
            var cx = RNG.Range(0, width);
            var cy = RNG.Range(0, height);
            if (gridMap.GetCell(cx, cy, out Cell? cell)) { if (cell!.Tiles.Last().ID == "white_tile") { continue; } }
            gridMap.AddTile(cx, cy, new Tile("crate_wood", 29, blockVision: true));
        }

        // Test (Add some crates in a line, with spaces between them)
        // for (int i = 0; i < 15; i++) {
        //     if (i % 2 == 0) { continue; }
        //     int cx = (int)Center.X - 7 + i;
        //     int cy = (int)Center.Y - 7;
        //     gridMap.AddTile(cx, cy, new Tile("crate_wood", 29, blockVision: true));
        // }

        // Build the light map
        lightMap = new byte[width * height];
        for (int i = 0; i < lightMap.Length; i++) {
            lightMap[i] = ambientLightLevel;
        }

        // Add a test light
        lights.Add(new Light(Center, 255, 24));
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

    // Update the lightMap array for all lights
    public void UpdateLightmap() {
        Array.Fill(lightMap, ambientLightLevel);

        foreach (var light in lights) {
            light.Position = Camera.GetCellPos();
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
    }
}