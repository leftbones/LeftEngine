namespace LeftEngine.World;

public enum WallType {

};

class Tile {
    public string ID { get; set; }

    // Tile Properties
    public int      Height { get; set; }            = 0;        // Pixel height of the tile, used to offset tiles on top of it
    public bool     IsFloor { get; set; }           = false;    // Tile is a floor tile
    public bool     IsWall { get; set; }            = false;    // Tile is a wall tile
    public bool     BlockVisionNS { get; set; }     = false;    // Tile blocks vision to the north and south
    public bool     BlockVisionEW { get; set; }     = false;    // Tile blocks vision to the east and west
    public bool     BlockMovementNS { get; set; }   = false;    // Tile blocks movement to the north and south
    public bool     BlockMovementEW { get; set; }   = false;    // Tile blocks movement to the east and west

    // Convenience Properties (read only)
    public bool     BlockVision => BlockVisionNS && BlockVisionEW;          // Tile blocks vision from all directions
    public bool     BlockMovement => BlockMovementNS && BlockMovementEW;    // Tile blocks movement from all directions

    public Tile(string id) {
        ID = id;
    }
}