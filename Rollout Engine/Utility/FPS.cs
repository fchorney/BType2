using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public FPS() : base(G.Game)
        {
            FrameRate = 0;
        }

        public override void Initialize()
        {
            TextWriter.Add("FPS");
            base.Initialize();
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

            TextWriter.Update("FPS",FrameRate.ToString());
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;
            base.Draw(gameTime);
        }
    }
}
