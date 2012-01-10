using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Drawing.Particle;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class ParticlesTest : Screen
    {
        private ParticleEmitter emitter;
        private Limiter limiter;
        private const int particleCount = 150;

        public override void Initialize()
        {
            TextWriter.Add("Particle Count");
            TextWriter.Add("Particle Buffer Count");
            TextWriter.Add("Enabled Particles");

            limiter = new Limiter(-1);

            emitter = new ParticleEmitter("Effector",new Animation(@"Sprites/Lensflare", 16, 16), null, particleCount) {X = 500, Y = 200};
            ScriptingEngine.Add("Effector", emitter);
            Add(emitter);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //TextWriter.Update("Particle Count", emitter.Count.ToString());
            //TextWriter.Update("Particle Buffer Count", emitter.BufferCount.ToString());
            //TextWriter.Update("Enabled Particles", (particleCount - emitter.BufferCount).ToString());

            limiter.Update(gameTime);
            if (limiter.Ready)
            {
                emitter.Emit(10);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {         
            G.SpriteBatch.Begin(Transition.Transform());
            base.Draw(gameTime);
            G.SpriteBatch.End();
        }
    }
}
