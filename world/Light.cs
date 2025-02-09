using Raylib_cs;
using System.Numerics;

namespace LeftEngine.World;

// The default angle of a light is 360 degrees, so it emites light in all directions, if angle is less than 360, you get a sort of light "cone"
// All of the numbers are rounded to fit within the grid, but a 45 degree angle is still 1/4 of a circle like you would expect
// Direction is only taken into account if a light's angle is less than 360

//   angle
//  |-----|
//
// ..XXXXX..  -
// .XXXXXXX.  |
// ..XXXXX..  | length
// ...XXX...  |
// ....X....  -
// ....@....

class Light {
    public Vector2 Position { get; set; }       // Cell position of the light
    public byte Intensity { get; set; }         // How much light the source emits
    public byte Length { get; set; }            // How far the light reaches (in cells)
    public int Angle { get; set; }              // The angle of the light's arc (in degrees, if applicable)
    public Vector2 Direction { get; set; }      // The direction the light is pointing (if applicable)
    public Color Color { get; set; }            // The color of the light

    public Light(Vector2 position, byte intensity, byte length, int angle=360, Vector2? direction=null, Color? color=null) {
        Position = position;
        Intensity = intensity;
        Length = length;
        Angle = angle;
        Direction = direction ?? Vector2.Zero;
        Color = color ?? Color.White;
    }
}