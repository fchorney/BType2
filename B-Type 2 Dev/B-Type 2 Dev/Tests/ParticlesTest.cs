using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Input;
using Rollout.Scripting;
using Rollout.Screens;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class ParticlesTest : Screen
    {
        //private ParticleEffect_A pEffect;
        private ParticleEmitter emitter;
        private IScriptingEngine scriptingEngine { get; set; }
        private Limiter limiter;

        public override void Initialize()
        {
            TextWriter.Add("Particle Count");
            TextWriter.Add("Particle Buffer Count");
            TextWriter.Add("Enabled Particles");

            limiter = new Limiter(-1);

            scriptingEngine = ScriptingEngine.Instance;
            emitter = new ParticleEmitter(20000) {Name = "Effector", X = 500, Y = 300};
            scriptingEngine.Add(emitter);

            //pEffect = new ParticleEffect_A(new Sprite(new Vector2(100, 100), new Animation(@"Sprites/Lensflare", 256, 256, 1)));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //pEffect.Update(gameTime);
            //TextWriter.Update("Particle Count", pEffect.Count.ToString());
            //TextWriter.Update("Particle Buffer Count", pEffect.BufferCount.ToString());
            //TextWriter.Update("Enabled Particles", (20000 - pEffect.BufferCount).ToString());
            limiter.Update(gameTime);
            if (limiter.Ready)
            {
                emitter.fire();
            }
            emitter.Update(gameTime);

            scriptingEngine.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

           
            //pEffect.Draw();
            
            G.SpriteBatch.Begin(Transition.Transform());
            emitter.Draw();

            G.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
