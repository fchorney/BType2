using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;

namespace Rollout.Utility
{
    public class FPS : DrawableGameComponent
    {
        public int FrameRate { get; set; }
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private SpriteBatch spriteBatch;
        private SpriteFont DefaultFont = G.Content.Load<SpriteFont>(@"SpriteFonts/Debug");

        public FPS() : base(G.Game)
        {
            FrameRate = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                FrameRate = frameCounter;
                frameCounter = 0;
            }

            frameCounter++;

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(DefaultFont, FrameRate + "FPS", new Vector2(4, 4), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
