using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rollout.Drawing
{
    class ParticleEffect
    {
        private List<Particle> particles;
        private List<Particle> particleBuffer;

        public ParticleEffect (int bufferSize = 0)
        {
            particles = new List<Particle>();
            particleBuffer = new List<Particle>();

            for (var i = 0; i < bufferSize; i++)
            {
                particleBuffer.Add(new Particle());
            }
        }

        public Particle CreateParticle(params double[] Parameters)
        {
            Particle p;

            if (particleBuffer.Count > 0)
            {
                p = particleBuffer[0];
                particleBuffer.RemoveAt(0);
                p.Init(Parameters);
            }
            else
            {
                p = new Particle(Parameters);
            }

            return p;
        }

        public virtual void Add(Particle p)
        {
            p.Enabled = true;
            particles.Add(p);
        }

        public virtual void Start() {}

        public virtual void Stop() {}

        public void Update(GameTime gameTime)
        {
            foreach (Particle p in particles.Where(p => p.Enabled))
            {
                p.Update(gameTime);

                if (p.TimeToLive > 0 && p.Age > p.TimeToLive)
                {
                    p.Enabled = false;
                    particleBuffer.Add(p);
                }
            }
        }

        public void Draw()
        {
            // LINQ is the fucking shit!
            foreach (Particle p in particles.Where(p => p.Enabled))
                p.Draw();
        }
    }
}
