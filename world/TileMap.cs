using System.Numerics;

namespace LeftEngine.World;

class Cell {
    public List<Tile> Tiles { get; set; }
    public bool BlocksLight { get; set; }

    public Cell() {
        Tiles = [];
    }
}

class TileMap {
    public int Width { get; private set; }
    public int Height { get; private set; }

    private readonly List<Cell> cells = [];

    public TileMap(int width, int height) {
        Width = width;
        Height = height; 

        for (int i = 0; i < width * height; i++) {
            cells.Add(new Cell());
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var t = new Tile("wood_floor");
                SetTile(x, y, 0, t);
            }
        }
    }

    public bool InBounds(int x, int y) {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool GetTile(Vector2 pos, int layer, out Tile? tile)  { return GetTile((int)pos.X, (int)pos.Y, layer, out tile); }
    public bool GetTile(int x, int y, int layer, out Tile? tile) {
        if (!InBounds(x, y)) { tile = null; return false; }
        tile = cells[x + y * Width].Tiles[layer];
        return true;
    }

    public bool SetTile(Vector2 pos, int layer, Tile tile)  { return SetTile((int)pos.X, (int)pos.Y, layer, tile); }
    public bool SetTile(int x, int y, int layer, Tile tile) {
        if (!InBounds(x, y)) { return false; }
        var cell = cells[x + y * Width];
        if (cell.Tiles.Count <= layer) {
            cell.Tiles.Add(tile);
        } else {
            cell.Tiles[layer] = tile;
        }
        return true;
    }

    public bool AddTile(Vector2 pos, Tile tile)  { return AddTile((int)pos.X, (int)pos.Y, tile); }
    public bool AddTile(int x, int y, Tile tile) {
        if (!InBounds(x, y)) { return false; }
        cells[x + y * Width].Tiles.Add(tile);
        return true;
    }

    public bool GetCell(Vector2 pos, out Cell? cell)  { return GetCell((int)pos.X, (int)pos.Y, out cell); }
    public bool GetCell(int x, int y, out Cell? cell) {
        if (!InBounds(x, y)) { cell = null; return false; }
        cell = cells[x + y * Width];
        return true;
    }
}