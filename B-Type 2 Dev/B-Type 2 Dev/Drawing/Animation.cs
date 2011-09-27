using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace B_Type_2_Dev.Drawing
{
    public class Frame
    {
        private String _AssetName;
        private Texture2D _Texture;
        private Rectangle _SourceRectangle;

        public virtual int Height
        {
            get { return _Texture.Height; }
        }

        public virtual int Width
        {
            get { return _Texture.Width; }
        }

        public virtual Texture2D Texture
        {
            get { return _Texture; }
        }

        public virtual Rectangle SourceRectangle
        {
            get { return _SourceRectangle; }
        }

        internal Frame() { }

        public Frame(string assetName, Rectangle sourceRectangle)
        {
            _AssetName = assetName;
            _SourceRectangle = sourceRectangle;
            LoadResources();
        }

        public void LoadResources()
        {
            // Load Texture
            //_Texture = G.Content.Load<Texture2D>(_AssetName);
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }

    public class Animation : Frame
    {
        public List<Frame> Frames;
        public int CurrentFrame;
        public double AnimationSpeed;
        private double _TimeInFrame;

        internal int _TotalFrames;

        public int TotalFrames
        {
            get { return _TotalFrames; }
        }

        public override Texture2D Texture
        {
            get { return Frames[CurrentFrame].Texture; }
        }

        public override Rectangle SourceRectangle
        {
            get { return Frames[CurrentFrame].SourceRectangle; }
        }

        public override void Update(GameTime gameTime)
        {
            _TimeInFrame += gameTime.ElapsedGameTime.TotalSeconds;
            if (AnimationSpeed > 0 && _TimeInFrame >= AnimationSpeed)
            {
                CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
                _TimeInFrame = 0;
            }
        }
    }

    public class AnimationFactory
    {
        public static Animation GenerateAnimation(string assetName, int frameWidth, int frameHeight, int animLength, double animSpeed)
        {
            Animation _Animation = new Animation();
            Texture2D _Texture = null; //G.Content.Load<Texture2D>(assetName);

            int Cols = _Texture.Width / frameWidth;
            int rows = _Texture.Height / frameHeight;

            _Animation.Frames = new List<Frame>();
            _Animation.CurrentFrame = 0;
            _Animation._TotalFrames = animLength; //Cols * Rows

            for (int j = 0; j < rows; j++)
                for (int i = 0; i < Cols && j * Cols + i < animLength; i++)
                {
                    Frame _Frame = new Frame(assetName, new Rectangle(i * frameWidth, j * frameHeight, frameWidth, frameHeight));
                    _Animation.Frames.Add(_Frame);
                }

            _Animation.AnimationSpeed = animSpeed;
            return _Animation;
        }
    }
}
