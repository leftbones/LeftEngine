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

    private readonly TileMap tileMap;
    private readonly byte[] lightMap;

    private readonly List<Light> lights = [];

    public Map(int width, int height) {
        Size = new Vector2(width, height);
        Center = new Vector2(width / 2, height / 2);

        // Set up the tile map
        tileMap = new TileMap(width, height);

        // Test (create a center area of white tiles)
        for (int x = 0; x < 9; x++) {
            for (int y = 0; y < 9; y++) {
                var cx = (int)Center.X - 4 + x;
                var cy = (int)Center.Y - 4 + y;
                SetTile(cx, cy, 0, new Tile("white_tile"));
            }
        }

        // Test (set the north and west edge tiles to walls)
        for (int i = 0; i < width; i++) {
            tileMap.AddTile(i, 0, new Tile("wall_stone_n"));
            tileMap.AddTile(0, i, new Tile("wall_stone_w"));
        }

        // Add some random crates
        for (int i = 0; i < 100; i++) {
            var cx = RNG.Range(0, width);
            var cy = RNG.Range(0, height);
            if (tileMap.GetCell(cx, cy, out Cell? cell)) { if (cell!.Tiles.Last().ID == "white_tile") { continue; } }
            tileMap.AddTile(cx, cy, new Tile("crate_wood", 29));
        }

        // Build the light map
        lightMap = new byte[width * height];
        for (int i = 0; i < lightMap.Length; i++) {
            lightMap[i] = ambientLightLevel;
        }

        // Add a test light
        lights.Add(new Light(Center, 240, 24));
    }

    // Set a tile at a given position
    public bool SetTile(Vector2 pos, int layer, Tile tile)  { return tileMap.SetTile((int)pos.X, (int)pos.Y, layer, tile); }
    public bool SetTile(int x, int y, int layer, Tile tile) {
        return tileMap.SetTile(x, y, layer, tile);
    }

    // Get at tile at a given position
    public bool GetTile(Vector2 pos, int layer, out Tile? tile)  { return tileMap.GetTile((int)pos.X, (int)pos.Y, layer, out tile); }
    public bool GetTile(int x, int y, int layer, out Tile? tile) {
        return tileMap.GetTile(x, y, layer, out tile);
    }

    // Get a cell at a given position
    public bool GetCell(Vector2 pos, out Cell? cell)  { return tileMap.GetCell((int)pos.X, (int)pos.Y, out cell); }
    public bool GetCell(int x, int y, out Cell? cell) {
        return tileMap.GetCell(x, y, out cell);
    }

    // Update the lightMap array based on proximity to a light source
    public void UpdateLightmap() {
        Array.Fill(lightMap, ambientLightLevel);
        foreach (var light in lights) {
            light.Position = Camera.GetTilePos();
            var cursorPos = Camera.GetCursorTilePos();
            var endPoint = light.Position + Vector2.Normalize(cursorPos - light.Position) * light.Distance;
            var linePoints = Algorithms.GetConePoints(light.Position, endPoint, 120);

            foreach (var point in linePoints) {
                if (tileMap.GetTile(point, 0, out Tile? tile)) {
                    float distance = Vector2.Distance(new Vector2(point.X, point.Y), light.Position);
                    byte currentLightLevel = lightMap[(int)point.X + (int)point.Y * Width];
                    byte newLightLevel = (byte)Math.Clamp(currentLightLevel + (light.Intensity * (1 - (distance / light.Distance))), ambientLightLevel, 255);
                    lightMap[(int)point.X + (int)point.Y * Width] = newLightLevel;
                }
            }
        }
    }

    // Render all tileMap in the map
    public void Render() {
        UpdateLightmap();
        var cursorPos = Camera.GetCursorTilePos();

        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                if (tileMap.GetCell(x, y, out Cell? cell)) {
                    var lightLevel = lightMap[x + y * Width] / 255f;
                    var tileColor = new Color(lightLevel, lightLevel, lightLevel);
                    var heightOffset = 0;

                    foreach (var tile in cell!.Tiles) {
                        var texture = AssetManager.Textures[tile.ID];
                        var tileX = (x - y) * (int)Global.GridSize.X;
                        var tileY = (x + y) * (int)Global.GridSize.Y;

                        DrawTexture(texture, tileX, tileY - heightOffset, tileColor);
                        heightOffset += tile.Height;
                    }
                }
            }
        }
    }
}