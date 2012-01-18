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

        private Effect Effect { get; set; }

        private RenderTarget2D BackBuffer { get; set; }
        private RenderTarget2D ScreenBuffer { get; set; }

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

            ScreenBuffer = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            BackBuffer = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(ScreenBuffer);

            Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transform.Value);

            GraphicsDevice.Clear(Color.Black);
        }

        public new void End()
        {
            base.End();
            HasBegun = false;

            Effect = Effect ?? G.Content.Load<Effect>(@"Effect/TestEffect");

            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            GraphicsDevice.SetRenderTarget(BackBuffer);

            Begin(0, BlendState.AlphaBlend, null, null, null, Effect);
            Draw(ScreenBuffer, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            base.End();

            GraphicsDevice.SetRenderTarget(null);

            Begin(0, BlendState.Opaque, null, null, null, null);
            Draw(ScreenBuffer, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            base.End();

            Begin(0, BlendState.AlphaBlend, null, null, null, null);
            Draw(BackBuffer, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            base.End();
        }

        public void DrawString(string text, int x, int y)
        {
            DrawString(DefaultFont, text, new Vector2(x,y), Color.White);
        }
    }
}