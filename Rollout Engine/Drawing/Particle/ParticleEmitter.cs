using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;

namespace Rollout.Drawing
{
    public class ParticleEmitter : IScriptable
    {
        protected List<Particle> particles;
        protected List<Particle> particleBuffer;

        public float X { get; set; }
        public float Y { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }

        private List<IAction> actions;
        public List<IAction> Actions
        {
            get { return actions ?? (actions = new List<IAction>()); }
        }

        public int Count
        {
            get { return particles.Count; }
        }

        public int BufferCount
        {
            get { return particleBuffer.Count; }
        }

        public ParticleEmitter (int bufferSize = 0)
        {
            particles = new List<Particle>();
            particleBuffer = new List<Particle>();
            Enabled = true;

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

        public void fire()
        {
            Particle particle = CreateParticle(RNG.Next(-200, 200), RNG.Next(-100, 100), RNG.Next(-100, 100));
            particle.Sprite = new Sprite(new Vector2(100, 100), new Animation(@"Sprites/Lensflare", 256, 256));
            particle.TimeToLive = 10;
            particle.X = X;
            particle.Y = Y;
            particle.Scale = RNG.Next(0, 20) / 60f;
            particle.Color = new Color(RNG.Next(10, 50), RNG.Next(3, 8), RNG.Next(0, 255));
            Add(particle);

            int pos = particles.IndexOf(particle);
            particle.Name = "ParticleWillie" + pos.ToString();
            ScriptingEngine.Instance.Add(particle);

            IAction moveloop = new RepeatAction(-1);
            int derp = pos % 4;
            if (derp == 0)
            {
                moveloop.AddAction(new MoveAction(particle.Name, new Vector2(200, 200), Time.ms(300)), true);
                moveloop.AddAction(new MoveAction(particle.Name, new Vector2(200, -200), Time.ms(100)), true);
            }
            else if (derp == 1)
            {
                moveloop.AddAction(new MoveAction(particle.Name, new Vector2(-200, -200), Time.ms(300)), true);
                //moveloop.AddAction(new MoveAction(particle.Name, new Vector2(200, 200), Time.ms(100)), true);
            }
            else if (derp == 2)
            {
                moveloop.AddAction(new MoveAction(particle.Name, new Vector2(-200, 200), Time.ms(300)), true);
                //moveloop.AddAction(new MoveAction(particle.Name, new Vector2(200, -200), Time.ms(100)), true);
            }
            else if (derp == 3)
            {
                moveloop.AddAction(new MoveAction(particle.Name, new Vector2(200, -200), Time.ms(300)), true);
                //moveloop.AddAction(new MoveAction(particle.Name, new Vector2(-200, 200), Time.ms(100)), true);
            }

            moveloop.Reset();
            particle.Actions.Add(moveloop);
        }

        public void fire2()
        {
            Particle particle = CreateParticle(RNG.Next(-200, 200), RNG.Next(-100, 100), RNG.Next(-100, 100));
            particle.Sprite = new Sprite(new Vector2(100, 100), new Animation(@"Sprites/Lensflare", 256, 256));
            particle.TimeToLive = 10;
            particle.X = X;
            particle.Y = Y;
            particle.Scale = RNG.Next(0, 20) / 60f;
            particle.Color = new Color(RNG.Next(10, 50), RNG.Next(3, 8), RNG.Next(0, 255));
            Add(particle);

            //int pos = particleBuffer.IndexOf(particle);
            int pos = particles.IndexOf(particle);
            particle.Name = Name + "-Particle-" + pos.ToString();
            ScriptingEngine.Instance.Add(particle);
            IAction action = new RepeatAction(2);
            action.AddAction(new MoveAction(particle.Name, new Vector2(0, -1000), 10f), true);
            particle.Actions.Add(action);
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
