using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rollout.Core
{
    public class ManagedSpriteBatch : SpriteBatch
    {
        private Boolean HasBegun { get; set; }

        public ManagedSpriteBatch()
            : base(G.Game.GraphicsDevice)
        {
            HasBegun = false;         
        }

        public new void Begin(Matrix? transform = null)
        {
            if (HasBegun)
                base.End();

            if (!transform.HasValue)
                transform = Matrix.Identity;

            Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transform.Value);
        }

        public new void End()
        {
            base.End();
            HasBegun = false;
        }
    }
}