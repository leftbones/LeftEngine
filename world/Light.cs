using Raylib_cs;
using System.Numerics;

namespace LeftEngine.World;

class Light {
    public Vector2 Position { get; set; }
    public byte Intensity { get; set; }
    public byte Distance { get; set; }
    public Color Color { get; set; }

    public Light(Vector2 position, byte intensity, byte distance, Color? color=null) {
        Position = position;
        Intensity = intensity;
        Distance = distance;
        Color = color ?? Color.White;
    }
}