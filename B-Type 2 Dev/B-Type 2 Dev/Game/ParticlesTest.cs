using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Drawing.Examples;
using Rollout.Input;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class ParticlesTest : DrawableGameComponent
    {
        private PlayerInput input;
        private TextWriter textWriter;
        private ParticleEffect_A pEffect;


        public ParticlesTest() : base(G.Game)
        {
        }

        public override void Initialize()
        {
            textWriter = new TextWriter(@"SpriteFonts/Debug");
            textWriter.Add("Particle Count");
            textWriter.Add("Particle Buffer Count");
            textWriter.Add("Enabled Particles");

            pEffect = new ParticleEffect_A(new Sprite(new Vector2(100, 100), "main", new Animation(@"Sprites/Lensflare", 256, 256, 1, new double[] { 1 })));
            G.Game.Components.Add(textWriter);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            pEffect.Update(gameTime);
            textWriter.Update("Particle Count", pEffect.Count.ToString());
            textWriter.Update("Particle Buffer Count", pEffect.BufferCount.ToString());
            textWriter.Update("Enabled Particles", (20000 - pEffect.BufferCount).ToString());
        }

        public override void Draw(GameTime gameTime)
        {

            G.SpriteBatch.Begin();
            pEffect.Draw();


            G.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
