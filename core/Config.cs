using Calcium;
using Newtonsoft.Json;

namespace LeftEngine.Core;

static class Config {
    // Window
    public static Vector2i WindowSize = new(800, 600);
    public static bool Fullscreen = false;

    // Input
    public static Dictionary<string, List<string>> Keymap = new();

    public static void LoadKeymap() {
        try {
            Keymap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("config/user_keymap.json"));
            Console.WriteLine("User keymap loaded");
        } catch {
            Keymap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("config/default_keymap.json"));
            Console.WriteLine("User keymap not found, default keymap loaded");
        }
    }
}