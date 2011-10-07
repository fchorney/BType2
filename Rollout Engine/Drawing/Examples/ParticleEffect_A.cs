using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Utility;

namespace Rollout.Drawing.Examples
{
    public class ParticleEffect_A : ParticleEffect
    {
        private Limiter limiter;
        private Sprite sprite;
        public int x { get; set; }
        public int y { get; set; }

        public ParticleEffect_A(Sprite sprite) : base(100000)
        {
            limiter = new Limiter(0);
            x = y = 0;
            this.sprite = sprite;
        }

        public override void Update(GameTime gameTime)
        {
            limiter.Update(gameTime);
            if (limiter.Ready)
            {
                for (var i = 0; i < 50; i++)
                {
                    // Params:
                    // 1: x-speed
                    // 2: y-speed
                    Particle particle = CreateParticle(RNG.Next(-200,200), RNG.Next(-100,100), RNG.Next(-100, 100));
                    particle.TimeToLive = 30;
                    particle.X = x;
                    particle.Y = y;
                    particle.Scale = 0.1f;

                    particle.Transform = null;
                    particle.Transform += (IParticle p) =>
                                              {
                                                  p.X = 500 + ((float) (p.Age*p.Params[1])%1000) +
                                                        (float) (Math.Cos(p.Age)*p.Params[0]);
                                              };
                    particle.Transform += (IParticle p) =>
                                              {
                                                  p.Y = 200 + ((float) (p.Age*p.Params[2])%1000) +
                                                        (float) (Math.Sin(p.Age)*p.Params[0]);
                                              };

                    particle.Sprite = sprite;
                    Add(particle);
                }
            }
            base.Update(gameTime);
        }
    }
}
