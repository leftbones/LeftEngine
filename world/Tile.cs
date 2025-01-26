namespace LeftEngine.World;

class Tile {
    public string ID { get; private set; }
    public int Height { get; private set; }
    public int MaxHeight { get; private set; }
    public bool BlockVision { get; private set; }
    public bool BlockMovement { get; private set; }

    public Tile(string id, int height=0, int maxHeight=999, bool blockVision=false, bool blockMovement=false) {
        ID = id;
        Height = height;
        MaxHeight = maxHeight;
        BlockVision = blockVision;
        BlockMovement = blockMovement;
    }
}