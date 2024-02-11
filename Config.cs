using Calcium;
using Newtonsoft.Json;

namespace LeftEngine;

static class Config {
    // Window
    public static Vector2i WindowSize = new(1280, 800);
    public static bool Fullscreen = false;

    // Input
    public static Dictionary<string, List<string>> Keymap = new();

    public static void LoadKeymap() {
        try {
            Keymap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("config/user_keymap.json"));
        } catch {
            Keymap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText("config/default_keymap.json"));
        }
    }
}