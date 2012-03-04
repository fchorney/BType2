using System;
using BloomPostprocess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rollout.Core
{
    public class ManagedSpriteBatch : SpriteBatch
    {
        private SpriteFont DefaultFont { get; set; }
        private Boolean HasBegun { get; set; }

        private BloomComponent bloom;

        public ManagedSpriteBatch()
            : base(G.Game.GraphicsDevice)
        {
            HasBegun = false;

            DefaultFont = G.Content.Load<SpriteFont>(@"SpriteFonts/Debug");
            bloom = new BloomComponent(this);

        }

        public new void Begin(Matrix? transform = null)
        {
            if (HasBegun)
                base.End();

            if (!transform.HasValue)
                transform = Matrix.Identity;

            bloom.BeginDraw();

            Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transform.Value);

            GraphicsDevice.Clear(Color.Black);

        }

        public new void End()
        {
            base.End();
            HasBegun = false;

            bloom.Draw();
            
        }

        public void DrawString(string text, Vector2 position, Color? color = null)
        {
            if (!color.HasValue)
            {
                color = Color.White;
            }

            DrawString(DefaultFont, text, position, color.Value);
        }
    }
}