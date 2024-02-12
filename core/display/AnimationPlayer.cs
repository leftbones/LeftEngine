using System.Numerics;
using Calcium;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

struct SpriteSheet {
    public Texture2D Texture;
    public Vector2i SpriteSize;
    
    public SpriteSheet(Texture2D texture, int sprite_size) : this(texture, new Vector2i(sprite_size, sprite_size)) { }
    public SpriteSheet(Texture2D texture, Vector2i sprite_size) {
        Texture = texture;
        SpriteSize = sprite_size;
    }

    public readonly Rectangle Get(Vector2i pos) {
        return Get(pos.X, pos.Y);
    }

    public readonly Rectangle Get(int x, int y) {
        return new Rectangle(x * SpriteSize.X, y * SpriteSize.Y, SpriteSize.X, SpriteSize.Y);
    }
}

struct Frame {
    public Rectangle Region;    // Location within the texture atlas
    public int Length;          // Length of this Frame in render frames (length of 15 would be 0.25 seconds at 60fps)

    public Frame(int x, int y, int w, int h, int length=15) : this(new Rectangle(x, y, w, h), length) { }
    public Frame(Rectangle region, int length=15) {
        Region = region;
        Length = length;
    }
}

class Animation {
    public string Name { get; private set; }                // Name used to look up this animation in the AnimationPlayer
    public SpriteSheet SpriteSheet { get; private set; }    // Texture containing the Animation Frames
    public List<Frame> Frames { get; private set; }         // List of Frames that make up this Animation
    public Vector2i Offset { get; private set; }            // Offset to apply when drawing this Animation
    public bool Loop { get; private set; }                  // If the animation should loop when it's finished

    // Actions
    public Action OnFinishedPlaying { get; set; }         // Action called when the Animation finishes playing

    // Create an Animation by giving the coordinates of the first Frame and how many Frames should follow
    public Animation(string name, SpriteSheet sprite_sheet, int x, int y, int frame_count, int frame_length=15, Vector2i? offset=null, bool loop=true) {
        Name = name;
        SpriteSheet = sprite_sheet;
        Offset = offset ?? Vector2i.Zero;
        Loop = loop;

        Frames = new List<Frame>();
        for (int i = 0; i < frame_count; i++) {
            Frames.Add(new Frame(SpriteSheet.Get(x + i, y), frame_length));
        }
    }

    // Create an Animation with a user-defined list of Frame coordinates inside the SpriteSheet
    public Animation(string name, SpriteSheet sprite_sheet, List<Vector2i> frames, int frame_length=15, Vector2i? offset=null, bool loop=true) {
        Name = name;
        SpriteSheet = sprite_sheet;
        Offset = offset ?? Vector2i.Zero;
        Loop = loop;

        Frames = new List<Frame>();
        foreach (var f in frames) {
            Frames.Add(new Frame(SpriteSheet.Get(f), frame_length));
        }
    }
}

class AnimationPlayer {
    public Dictionary<string, Animation> Animations { get; private set; }   // List of Animations managed by this AnimationPlayer
    public Animation Animation { get; private set; }                        // Currently playing Animation
    public bool Playing { get; private set; }                               // If the Animation is playing or paused

    public Frame Frame { get { return Animation.Frames[Progress]; } }       // Current Frame in the current Animation


    private int FrameTimer = 0;
    private int Progress = 0;

    public AnimationPlayer(List<Animation> animations, bool autoplay=true) {
        Animations = new Dictionary<string, Animation>();
        foreach (var anim in animations) {
            Animations.Add(anim.Name, anim);
        }

        Animation = animations[0];
        Playing = autoplay;
    }

    // Play the Animation matching the given name, if it exists
    public void Play(string name) {
        if (Animations.ContainsKey(name)) {
            Animation = Animations[name];
            FrameTimer = 0;
            Progress = 0;
            Playing = true;
        }
    }

    // Resume the current Animation (if not playing)
    public void Play() {
        Playing = true;
    }

    // Pause the current Animation, retaining the Frame, Progress, and FrameTimer states
    public void Pause() {
        Playing = false;
    }

    // Set the Frame of the current Animation manually
    public void SetFrame(int index) {
        Progress = index;
    }

    // Set the current Animation without automatically playing it
    public void SetAnimation(string name) {
        if (Animations.ContainsKey(name)) {
            Animation = Animations[name];
            FrameTimer = 0;
            Progress = 0;
        }
    }

    // If the animation is playing, progress the frame timer and switch frames when necessary
    public void Update() {
        if (Playing) {
            FrameTimer++;
            if (FrameTimer == Frame.Length) {
                FrameTimer = 0;
                Progress++;

                if (Progress == Animation.Frames.Count) {
                    Playing = Animation.Loop;
                    Progress = 0;

                    Animation.OnFinishedPlaying?.Invoke();
                }
            }
        }
    }

    // Draw the current Frame of the Animation to the screen
    public void Draw(Vector2i pos) { Draw(pos.X, pos.Y); }
    public void Draw(Vector2 pos) { Draw((int)pos.X, (int)pos.Y); }
    public void Draw(int x, int y) {
        var Dest = new Rectangle(x + Animation.Offset.X, y + Animation.Offset.Y, Frame.Region.Width * Global.SpriteScale, Frame.Region.Height * Global.SpriteScale);
        DrawTexturePro(Animation.SpriteSheet.Texture, Frame.Region, Dest, Vector2.Zero, 0.0f, Color.WHITE);
    }
}
