using System.Collections.Generic;
using Rollout.Core;
using Rollout.Scripting;
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

    public class Sprite : DrawableGameObject, IScriptable
    {
        #region variables

        protected Vector2 position;

        private List<IAction> actions;
        private Animation animation;
        private Dictionary<string, Animation> animations;

        public Color Color { get; set; }
        public string Name { get; set; }

        public List<IAction> Actions
        {
            get { return actions ?? (actions = new List<IAction>()); }
        }

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

        /// <summary>
        /// Create a new Sprite with a position.
        /// Optionally pass in a default animation name and animation.
        /// </summary>
        /// <param name="startPosition">Required: Start position of sprite</param>
        /// <param name="animationName">Optional: Default animation name (Required with animation parameter)</param>
        /// <param name="animation">Optional: Default animation (Required with animationName parameter)</param>
        public Sprite(Vector2 startPosition, Animation animation = null, string animationName = "main")
        {
            animations = new Dictionary<string, Animation>();
            OffsetX = startPosition.X;
            OffsetY = startPosition.Y;
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;
            Enabled = true;

            if (animation != null)
            {
                AddAnimation(animationName, animation);
            }
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
            base.Update(gameTime);
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
