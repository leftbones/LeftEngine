# LeftEngine
I'm developing LeftEngine for my own personal use, it's a 2D tile-based isometric game engine that supports using 3D models in 2D space. 

### Features
* GridMap contains a list of Cells, each Cell stores Tiles in the order they should be rendered
* AssetManager loads and caches textures into a dictionary where they can be retrieved when needed using their file name (minus the extension)
* Input system handles all input monitoring and tracks pressed, released, and held keys
* Camera moves smoothly and can be used to translate the cursor screen position to a cell position in the Map
* Lighting system supports multiple lights, will eventually support colored lights as well

### To Do
* Allow tiles to block light/FOV
* Split the Map into Chunks, each with its own GridMap(?)
* Render a Chunk's GridMap to a texture and cache it, only re-render when necessary
* Rendering of 3D models to a render texture
* Allow rendered 3D models to be cached into a Chunk's render texture
* Expose core parts of the engine to a public Lua API for implementing game logic (and mods) with simple scripts

### Resources
- [raylib-cs](https://github.com/raylib-cs/raylib-cs) for windowing, rendering, input monitoring, all that stuff
- [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) and [rlImGui-cs](https://github.com/raylib-extras/rlImGui-cs) for the lightweight and easy to implement UI
- [MoonSharp](https://github.com/moonsharp-devs/moonsharp) for the scripting API that will be implemented eventually