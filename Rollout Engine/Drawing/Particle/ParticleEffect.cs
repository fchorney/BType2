using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rollout.Drawing
{
    public class ParticleEffect
    {
        protected List<Particle> particles;
        protected List<Particle> particleBuffer;

        public int Count
        {
            get { return particles.Count; }
        }

        public int BufferCount
        {
            get { return particleBuffer.Count; }
        }

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
            int index = particles.IndexOf(p);
            if (index >= 0)
            {
                particles[index].Enabled = true;

            }
            else
            {
                p.Enabled = true;
                particles.Add(p);
            }
        }

        public virtual void Start() {}

        public virtual void Stop() {}

        public virtual void Update(GameTime gameTime)
        {
            foreach (Particle p in particles.AsParallel().Where(p => p.Enabled))
            //foreach (Particle p in particles.Where(p => p.Enabled))
            {
                p.Update(gameTime);

                if (p.TimeToLive > 0 && p.Age > p.TimeToLive)
                {
                    p.Enabled = false;
                    particleBuffer.Add(p);
                }
            }
        }

        public virtual void Draw()
        {
            // LINQ is the fucking shit!
            foreach (Particle p in particles.AsParallel().Where(p => p.Enabled))
                p.Draw();
        }
    }
}
