using System.Collections.Generic;
using B_Type_2_Dev.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace B_Type_2_Dev.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }

    class Sprite : ITransformable
    {
        private Animation frame;
        private Vector2 position;
        private Dictionary<string, Animation> animations;

        public Animation Animation
        {
            get { return frame; }
        }

        public int Height
        {
            get { return frame.SourceRectangle.Height; }
        }

        public int Width
        {
            get { return frame.SourceRectangle.Width; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }

        public Sprite(Vector2 startPosition)
        {
            animations = new Dictionary<string, Animation>();
            position = startPosition;
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;
        }

        public void Update(GameTime gameTime)
        {
            frame.Update(gameTime);
        }

        public void Draw(float x, float y, Color color, float scale, float rotation)
        {
            Vector2 texCenter = new Vector2(Width/2, Height/2);
            G.SpriteBatch.Draw(frame.Texture, new Vector2(x + texCenter.X, y + texCenter.Y),frame.CurrentFrame.SourceRectangle,color, rotation, texCenter, scale, SpriteEffects.None, 0);
        }

        public void Draw()
        {
            Draw(position.X, position.Y, Color, Scale, Rotation);
        }

        public void SetAnimation(string animationName)
        {
            frame = animations[animationName];
            frame.Reset();
        }

        public void Pause()
        {
            frame.Paused = true;
        }

        public void UnPause()
        {
            frame.Paused = false;
        }

        public void ReStart()
        {
            frame.Reset();
        }

        public bool isPaused()
        {
            return frame.Paused;
        }

        public void AddAnimation(string animationName, Animation animation)
        {
            animations.Add(animationName, animation);
            // If this is the first animation added
            // Set it to the current running animation
            if (animations.Count == 1)
                SetAnimation(animationName);
        }
    }
}
