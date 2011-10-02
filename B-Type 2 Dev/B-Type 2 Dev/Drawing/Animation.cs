using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using B_Type_2_Dev.Globals;

namespace B_Type_2_Dev.Drawing
{

    //public class Frame
    //{
    //    private String _AssetName;
    //    private Texture2D _Texture;
    //    private Rectangle _SourceRectangle;

    //    public virtual int Height
    //    {
    //        get { return _Texture.Height; }
    //    }

    //    public virtual int Width
    //    {
    //        get { return _Texture.Width; }
    //    }

    //    public virtual Texture2D Texture
    //    {
    //        get { return _Texture; }
    //    }

    //    public virtual Rectangle SourceRectangle
    //    {
    //        get { return _SourceRectangle; }
    //    }

    //    internal Frame() { }

    //    public Frame(string assetName, Rectangle sourceRectangle)
    //    {
    //        _AssetName = assetName;
    //        _SourceRectangle = sourceRectangle;
    //        LoadResources();
    //    }

    //    public void LoadResources()
    //    {
    //        // Load Texture
    //        _Texture = G.Content.Load<Texture2D>(_AssetName);
    //    }

    //    public virtual void Update(GameTime gameTime)
    //    {

    //    }
    //}

    //public class Animation : Frame
    //{
    //    public List<Frame> Frames;
    //    public int CurrentFrame;
    //    public double AnimationSpeed;
    //    private double _TimeInFrame;

    //    internal int _TotalFrames;

    //    public int TotalFrames
    //    {
    //        get { return _TotalFrames; }
    //    }

    //    public override Texture2D Texture
    //    {
    //        get { return Frames[CurrentFrame].Texture; }
    //    }

    //    public override Rectangle SourceRectangle
    //    {
    //        get { return Frames[CurrentFrame].SourceRectangle; }
    //    }

    //    public override void Update(GameTime gameTime)
    //    {
    //        _TimeInFrame += gameTime.ElapsedGameTime.TotalSeconds;
    //        if (AnimationSpeed > 0 && _TimeInFrame >= AnimationSpeed)
    //        {
    //            CurrentFrame++;
    //            if (CurrentFrame == TotalFrames)
    //                CurrentFrame = 0;
    //            _TimeInFrame = 0;
    //        }
    //    }
    //}

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
        //public bool Loop { get; set; } Put somewhere else

        private double currentDuration;
        //private double totalDuration;
        private double timeInFrame;
        internal int totalFrames;

        public Frame CurrentFrame
        {
            get { return Frames[FrameIndex]; }
        }

        public Animation(string assetName)
        {
            Texture = G.Content.Load<Texture2D>(assetName);
            FrameIndex = 0;
            timeInFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (currentDuration == 0)
                currentDuration = Frames[FrameIndex].Duration;
            timeInFrame += gameTime.ElapsedGameTime.TotalSeconds;
            
            if (timeInFrame >= currentDuration)
            {
                FrameIndex++;
                if (FrameIndex >= Frames.Count)
                {
                    FrameIndex = 0;
                }
                currentDuration += Frames[FrameIndex].Duration;
            }
        }

    }

    public class AnimationFactory
    {
        public static Animation GenerateAnimation(string assetName, int frameWidth, int frameHeight, int numFrames,double[] durations)
        {
            Animation _Animation = new Animation(assetName);

            int cols = _Animation.Texture.Width / frameWidth;
            int rows = _Animation.Texture.Height / frameHeight;

            _Animation.Frames = new List<Frame>();
            _Animation.FrameIndex = 0;
            _Animation.totalFrames = numFrames;
            
            for (int j = 0; j < rows; j++)
                for (int i = 0; i < cols && j * cols + i < numFrames; i++)
                {
                    Frame _Frame = new Frame(new Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight),durations[j * cols + i]);
                    _Animation.Frames.Add(_Frame);
                }

            return _Animation;
        }
    }
}
