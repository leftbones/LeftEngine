using System.Numerics;
using rlImGui_cs;
using ImGuiNET;

namespace LeftEngine.Core;

static class UI {
    private static bool showInfoOverlay = true;

    private static readonly ImGuiWindowFlags overlayFlags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoBringToFrontOnFocus;

    public static void Init() {
        rlImGui.Setup(true);
        ImGui.StyleColorsDark();
    }

    public static void Update() {

    }

    public static void Draw() {
        rlImGui.Begin();

        // Debug
        if (showInfoOverlay) { ShowInfoOverlay(); }


        ImGui.End();
        rlImGui.End();
    }

    public static void Exit() {
        rlImGui.Shutdown();
    }

    // Debug stats overlay
    static void ShowInfoOverlay() {
        ImGui.SetNextWindowPos(new Vector2(10, 10));
        ImGui.SetNextWindowBgAlpha(0.5f);
        if (ImGui.Begin("Debug Info", ref showInfoOverlay, overlayFlags)) {
            var mouseTile = Camera.GetCursorCellPos();

            ImGui.Text($"FPS: {GameWindow.FPS}");
            ImGui.Text($"Cell: {mouseTile.X}, {mouseTile.Y}");
        }
    }
}