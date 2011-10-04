using System.Collections.Generic;
using B_Type_2_Dev.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace B_Type_2_Dev.Drawing
{
    public class Frame
    {
        public Rectangle SourceRectangle { get; set; }
        public double Duration { get; set; }   
        
        public Frame (Rectangle sourceRectangle, double duration)
        {
            SourceRectangle = sourceRectangle;
            Duration = duration;
        }
    }

    public class Animation
    {
        public List<Frame> Frames { get; set; }
        public int FrameIndex { get; set; }
        public Texture2D Texture { get; private set; }
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

        public Animation(string assetName, int frameWidth, int frameHeight, int numFrames,double[] durations, bool loop = true, int loopCount = -1)
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
                for (int i = 0; i < cols && j * cols + i < numFrames; i++)
                    Frames.Add(new Frame(new Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight), durations[j * cols + i]));
        }

        public void Update(GameTime gameTime)
        {
            if (!Paused && (Loop || (!Loop && currentLoopCount < 1)))
            {
                if (loopCount == -1 || (loopCount > -1 && currentLoopCount < loopCount))
                {
                    timeInFrame += gameTime.ElapsedGameTime.TotalSeconds;
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
    }
}
