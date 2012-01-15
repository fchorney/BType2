using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rollout.Core.GameObject;

namespace Rollout.Drawing.Sprites
{
    public class Sprite : DrawableGameObject, ICollidable
    {
        #region variables

        protected Vector2 position;

        private Animation animation;
        private Dictionary<string, Animation> animations;

        public Color Color { get; set; }
        public string Name { get; set; }

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

        #endregion

        public Sprite()
        {
            animations = new Dictionary<string, Animation>();
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;
            Enabled = true;
        }

        /// <summary>
        /// Create a new Sprite with a position.
        /// Optionally pass in a default animation name and animation.
        /// </summary>
        /// <param name="startPosition">Required: Start position of sprite</param>
        /// <param name="animationName">Optional: Default animation name (Required with animation parameter)</param>
        /// <param name="animation">Optional: Default animation (Required with animationName parameter)</param>
        public Sprite(Vector2 startPosition, Animation animation = null, string animationName = "main") : this()
        {
            OffsetX = startPosition.X;
            OffsetY = startPosition.Y;

            if (animation != null)
            {
                AddAnimation(animationName, animation);
            }
        }

        public override void Update(GameTime gameTime)
        {


            animation.Update(gameTime);
            base.Update(gameTime);

            if (Shape != null)
            {
                Shape.X = X;
                Shape.Y = Y;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(X, Y, Color, Scale, Rotation);
            base.Draw(gameTime);
        }

        public void Draw(float x, float y, Color color, float scale, float rotation)
        {
            Vector2 texCenter = new Vector2(W/2, H/2);
            //Vector2 texCenter = new Vector2(0, 0); //scale from top left
            G.SpriteBatch.Draw(animation.Texture, new Vector2(x + texCenter.X, y + texCenter.Y),animation.CurrentFrame.SourceRectangle,color, rotation, texCenter, scale, SpriteEffects.None, 0);
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

        public bool Paused()
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

        public IShape Shape { get; set; }

        public bool Primary { get; set; }
    }
}
