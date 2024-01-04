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
    public Texture2D Texture { get; private set; }      // Texture containing the Animation Frames
    public List<Frame> Frames { get; private set; }     // List of Frames that make up this Animation
    public bool Loop { get; private set; }              // If the animation should loop when it's finished

    public Action OnAnimationFinished { get; set; }     // Action called when the Animation finishes playing

    // Create an Animation and have the list of Frames populated automatically (Less customizable, but less verbose)
    public Animation(string name, Texture2D texture, int x, int y, int frame_width, int frame_height, int frame_count, int frame_length=15, bool loop=true) {
        Frames = new List<Frame>();
        for (int i = 0; i < frame_count; i++) {
            Frames.Add(new Frame(x + (frame_width * i), y, frame_width, frame_height, frame_length));
        }
        Name = name;
        Texture = texture;
        Loop = loop;
    }

    // Create an Animation with a user-defined list of Frames (More customizable, but more verboase)
    public Animation(string name, Texture2D texture, List<Frame> frames, bool loop=true) {
        Name = name;
        Texture = texture;
        Frames = frames;
        Loop = loop;
    }
}

class AnimationPlayer {
    public Dictionary<string, Animation> Animations { get; private set; }   // List of Animations managed by this AnimationPlayer
    public Animation Animation { get; private set; }                        // Currently playing Animation
    public bool Playing { get; private set; }                               // If the Animation is playing or paused

    public Frame Frame { get { return Animation.Frames[Progress]; } }       // Current Frame in the current Animation


    private int FrameTimer = 0;
    private int Progress = 0;

    public AnimationPlayer(List<Animation> animations) {
        Animations = new Dictionary<string, Animation>();
        foreach (var anim in animations) {
            Animations.Add(anim.Name, anim);
        }

        Animation = animations[0];
        Playing = true;
    }

    // Play the Animation matching the given name, if it exists
    public void Play(string name) {
        if (Animations.ContainsKey(name)) {
            Animation = Animations[name];
            FrameTimer = 0;
            Progress = 0;
        }
    }

    // Resume the current Animation when paused
    public void Play() {
        Playing = true;
    }

    // Pause the current Animation, retaining the Frame, Progress, and FrameTimer states
    public void Pause() {
        Playing = false;
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

                    Animation.OnAnimationFinished?.Invoke();
                }
            }
        }
    }

    // Draw the current Frame of the Animation to the screen
    public void Draw() {
        var Dest = new Rectangle(128, 128, Frame.Region.Width * 2, Frame.Region.Height * 2);
        DrawTexturePro(Animation.Texture, Frame.Region, Dest, Vector2.Zero, 0.0f, Color.WHITE);
    }
}