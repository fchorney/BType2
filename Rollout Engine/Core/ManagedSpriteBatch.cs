using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rollout.Core
{
    public class ManagedSpriteBatch : SpriteBatch
    {
        private SpriteFont DefaultFont { get; set; }
        private Boolean HasBegun { get; set; }

        public ManagedSpriteBatch()
            : base(G.Game.GraphicsDevice)
        {
            HasBegun = false;

            DefaultFont = G.Content.Load<SpriteFont>(@"SpriteFonts/Debug");

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

        public void DrawString(string text, int x, int y)
        {
            DrawString(DefaultFont, text, new Vector2(x,y), Color.White);
        }
    }
}