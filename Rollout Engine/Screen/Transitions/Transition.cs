using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;
using Rollout.Input;

namespace Rollout.Screens
{

    public class Transition
    {

        public TimeSpan OnTime { get; set; }

        public TimeSpan OffTime { get; set; }

        public float Position { get; set; }

        public byte Alpha
        {
            get { return (byte)(Position * 255); }
        }

        public bool Update(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float delta;

            if (time == TimeSpan.Zero)
                delta = 1;
            else
                delta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            this.Position += delta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (this.Position <= 0)) ||
                ((direction > 0) && (this.Position >= 1)))
            {
                this.Position = MathHelper.Clamp(this.Position, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        public Matrix Transform()
        {
            Matrix transform = Matrix.Identity;
            transform.Translation = new Vector3(G.SpriteBatch.GraphicsDevice.Viewport.Width * Position, 0, 0);
            return transform;
        }


    }
}