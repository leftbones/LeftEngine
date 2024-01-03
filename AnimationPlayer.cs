using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace LeftEngine;

struct Frame {
    public Rectangle Region;    // Location within the texture atlas
    public int Length;          // Length of this Frame in render frames (length of 15 would be 0.25 seconds at 60fps)

    public Frame(int x, int y, int w, int h, int length=15) {
        Region = new Rectangle(x, y, w, h);
        Length = length;
    }
}

class Animation {
    public string Name { get; private set; }            // Name used to look up this animation in the AnimationPlayer
    public List<Frame> Frames { get; private set; }     // List of Frames that make up this Animation

    public Animation(string name, List<Frame> frames) {
        Name = name;
        Frames = frames;
    }
}

class AnimationPlayer {
    public Dictionary<string, Animation> Animations { get; private set; }   // List of Animations managed by this AnimationPlayer
    public Animation Animation { get; private set; }                        // Currently playing Animation

    public Frame Frame { get { return Animation.Frames[Progress]; } }       // Current Frame in the current Animation

    private int FrameTimer = 0;
    private int Progress = 0;

    public AnimationPlayer(List<Animation> animations) {
        Animations = new Dictionary<string, Animation>();
        foreach (var anim in animations) {
            Animations.Add(anim.Name, anim);
        }

        Animation = animations[0];
    }

    public void Update() {
        FrameTimer++;
        if (FrameTimer == Frame.Length) {
            FrameTimer = 0;
            Progress++;

            if (Progress == Animation.Frames.Count) {
                Progress = 0;
            }
        }
    }

    public void Draw(Texture2D Texture) {
        var Dest = new Rectangle(128, 128, Frame.Region.Width * 2, Frame.Region.Height * 2);
        DrawTexturePro(Texture, Frame.Region, Dest, Vector2.Zero, 0.0f, Color.WHITE);
    }
}