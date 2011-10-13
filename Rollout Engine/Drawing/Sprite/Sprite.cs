using System.Collections.Generic;
using Rollout.Collision;
using Rollout.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rollout.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }

    public class Sprite : ITransformable
    {
        private Vector2 position;
        private Animation animation;
        private Dictionary<string, Animation> animations;

        public Animation Animation
        {
            get { return animation; }
        }

        public int H
        {
            get { return animation.SourceRectangle.Height; }
        }

        public int W
        {
            get { return animation.SourceRectangle.Width; }
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

        /// <summary>
        /// Create a new Sprite with a position.
        /// Optionally pass in a default animation name and animation.
        /// </summary>
        /// <param name="startPosition">Required: Start position of sprite</param>
        /// <param name="animationName">Optional: Default animation name (Required with animation parameter)</param>
        /// <param name="animation">Optional: Default animation (Required with animationName parameter)</param>
        public Sprite(Vector2 startPosition, string animationName = null, Animation animation = null)
        {
            animations = new Dictionary<string, Animation>();
            position = startPosition;
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;

            if (animationName != null && animation != null)
            {
                AddAnimation(animationName, animation);
            }
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(float x, float y, Color color, float scale, float rotation)
        {
            Vector2 texCenter = new Vector2(W/2, H/2);
            G.SpriteBatch.Draw(animation.Texture, new Vector2(x + texCenter.X, y + texCenter.Y),animation.CurrentFrame.SourceRectangle,color, rotation, texCenter, scale, SpriteEffects.None, 0);
        }

        public void Draw()
        {
            Draw(position.X, position.Y, Color, Scale, Rotation);
        }

        public void SetAnimation(string animationName)
        {
            animation = animations[animationName];
            animation.Reset();
        }

        public void Pause()
        {
            animation.Paused = true;
        }

        public void UnPause()
        {
            animation.Paused = false;
        }

        public void ReStart()
        {
            animation.Reset();
        }

        public bool isPaused()
        {
            return animation.Paused;
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
