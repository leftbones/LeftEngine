namespace LeftEngine.World;

public enum TileTags {
    None,
    BlockVision,
    BlockVisionNS,
    BlockVisionEW,
    BlockMovement,
    BlockMovementNS,
    BlockMovementEW,
};

class Tile {
    public string ID { get; private set; }
    public int Height { get; private set; }
    public int MaxHeight { get; private set; }

    public TileTags Tags { get; set; }

    public Tile(string id, int height=0, int maxHeight=999, TileTags tags=TileTags.None) {
        ID = id;
        Height = height;
        MaxHeight = maxHeight;
        Tags = tags;
    }
}