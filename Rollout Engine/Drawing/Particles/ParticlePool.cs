using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Rollout.Core.GameObject;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;

namespace Rollout.Drawing
{

    public class ParticlePool<TParticle> : DrawableGameObject where TParticle : IParticle
    {

        List<TParticle> Pool { get; set; }
        List<TParticle> Buffer { get; set; }

        public ParticlePool(int bufferSize = 0)
        {
            Pool = new List<TParticle>();
            Buffer = new List<TParticle>();

            for (int i = 0; i < bufferSize; i++)
            {
                Buffer.Add(CreateParticle());
            }
        }

        public TParticle GetParticle()
        {
            TParticle p;

            if (Buffer.Count > 0)
            {
                p = Buffer[0];
                Buffer.RemoveAt(0);
            }
            else
            {
                p = CreateParticle();
                Pool.Add(p);
            }

            return p;
        }

        public TParticle CreateParticle()
        {
            var p = Activator.CreateInstance<TParticle>();

            return p;
        }


        public override void Update(GameTime gameTime)
        {
            foreach (var p in Pool.Where(p => p.Enabled))
            {
                p.Update(gameTime);

                if (p.TimeToLive > 0 && p.Age > p.TimeToLive)
                {
                    p.Enabled = false;
                    Buffer.Add(p);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var p in Pool.AsParallel().Where(p => p.Enabled))
                p.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
