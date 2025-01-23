using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine.Core;

static class AssetManager {
    public static readonly Dictionary<string, Texture2D> Icons = [];
    public static readonly Dictionary<string, Texture2D> Textures = [];

    public static void Init() {
        // Load Icons
        Icons.Add("empty", new Texture2D());
        foreach (var file in Directory.GetFiles("assets/icons")) {
            var texture = LoadTexture(file);
            var name = Path.GetFileNameWithoutExtension(file);
            Icons.Add(name, texture);
        }

        // Load Tiles
        Textures.Add("empty", new Texture2D());
        foreach (var file in Directory.GetFiles("assets/tiles")) {
            var texture = LoadTexture(file);
            var name = Path.GetFileNameWithoutExtension(file);
            Textures.Add(name, texture);
        }
    }
}