using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;

namespace Rollout.Drawing.Sprites
{
    public class Frame
    {
        public int SourceIndex { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public int Duration { get; set; }   
        
        public Frame (Rectangle sourceRectangle, int duration = 1000)
        {
            SourceRectangle = sourceRectangle;
            Duration = duration;
        }
    }

    public class Animation
    {
        public List<Frame> Frames { get; set; }
        public int FrameIndex { get; set; }
        public Texture2D Texture { get; set; }
        public bool Loop { get; set; }
        public bool Paused { get; set; }

        private int currentLoopCount;
        private int loopCount;
        private double currentDuration;
        private double timeInFrame;

        internal int totalFrames;

        public Frame CurrentFrame
        {
            get { return Frames[FrameIndex]; }
        }

        public Rectangle SourceRectangle
        {
            get { return CurrentFrame.SourceRectangle; }
        }

        public Animation()
        {
            
        }

        public Animation(string assetName, int frameWidth, int frameHeight, int numFrames = 1, double[] durations = null, bool loop = true, int loopCount = -1)
        {
            Texture = G.Content.Load<Texture2D>(assetName);
            GenerateFrames(frameWidth,frameHeight,numFrames,durations);
            Paused = false;
            Loop = loop;
            this.loopCount = loopCount;
            Reset();
        }

        private void GenerateFrames(int frameWidth, int frameHeight, int numFrames, double[] durations)
        {
            int cols = Texture.Width / frameWidth;
            int rows = Texture.Height / frameHeight;

            Frames = new List<Frame>();
            totalFrames = numFrames;

            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < cols && j*cols + i < numFrames; i++)
                {
                    Frame newFrame = new Frame(new Rectangle(i*frameWidth, j*frameHeight, frameWidth, frameHeight));
                    if (durations != null)
                        newFrame.Duration = (int)(durations[j * cols + i] * 10);
                    Frames.Add(newFrame);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Paused && (Loop || (!Loop && currentLoopCount < 1)))
            {
                if (loopCount == -1 || (loopCount > -1 && currentLoopCount < loopCount))
                {
                    timeInFrame += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeInFrame >= currentDuration)
                    {
                        FrameIndex++;
                        if (FrameIndex >= Frames.Count)
                        {
                            FrameIndex = 0;
                            currentLoopCount++;
                        }
                        currentDuration += Frames[FrameIndex].Duration;
                    }
                }
            }
        }

        public void Reset()
        {
            FrameIndex = 0;
            timeInFrame = 0;
            currentLoopCount = 0;
            currentDuration = Frames[FrameIndex].Duration;
        }

        public static Animation Load(string name)
        {
            return AnimationLoader.Animations.ContainsKey(name) ? AnimationLoader.Animations[name] : null;
        }
    }

    public class AnimationLoader
    {
        private static Dictionary<string, Animation> animations; 
        internal static Dictionary<string, Animation> Animations 
        {
            get 
            { 
                animations = animations ?? new Dictionary<string, Animation>();
                return animations;
            }
        }

        public static void Test()
        {
            var animationInfo = new AnimationInfo
                                    {
                                        Name = "player",
                                        AssetName = @"Sprites/spaceship2",
                                        Width = 64,
                                        Height = 64,
                                        Frames = new List<FrameInfo>
                                                     {
                                                         new FrameInfo() {Index = 0, Duration = 300},
                                                         new FrameInfo() {Index = 1, Duration = 300}
                                                     }
                                    };

            LoadAnimation(animationInfo);

        }

        private static void LoadAnimation(AnimationInfo animInfo)
        {
            var animation = new Animation {Texture = G.Content.Load<Texture2D>(animInfo.AssetName), Frames = new List<Frame>()};

            foreach (var frameInfo in animInfo.Frames)
            {
                var sourceRect = GetIndexSourceRectangle(animation.Texture.Width, animation.Texture.Height,
                                                         animInfo.Width, animInfo.Height, frameInfo.Index);
                var frame = new Frame(sourceRect, frameInfo.Duration);
                animation.Frames.Add(frame);
            }

            Animations.Add(animInfo.Name, animation);

        }

        private static Rectangle GetIndexSourceRectangle(int sourceWidth, int sourceHeight, int frameWidth, int frameHeight, int index)
        {
            int cols = sourceWidth / frameWidth;
            int rows = sourceHeight / frameHeight;

            int row = index/cols;
            int col = index%rows;

            return new Rectangle(col*frameWidth, row*frameHeight, frameWidth, frameHeight);
        }

        class AnimationInfo
        {
            public string Name { get; set; }
            public string AssetName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public List<FrameInfo> Frames { get; set; }
        }

        class FrameInfo
        {
            public int Index { get; set; }
            public int Duration { get; set; }
        }
    }
}
