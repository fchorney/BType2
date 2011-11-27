using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;

namespace Rollout.Drawing
{
    public class ParticleEmitter : DrawableGameObject, IScriptable
    {
        private List<Particle> particles;
        private List<Particle> particleBuffer;
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
                particleBuffer.Add(new Particle(new Vector2(0, 0), new Animation(@"Sprites/Lensflare", 256, 256)));
            }
        }

        public Particle CreateParticle()
        {
            Particle p;

            if (particleBuffer.Count > 0)
            {
                p = particleBuffer[0];
                particleBuffer.RemoveAt(0);
                p.Initialize();
                p.Enabled = true;
            }
            else
            {
                p = new Particle(new Vector2(0, 0), new Animation(@"Sprites/Lensflare", 256, 256));
            }

            return p;
        }

        public void Fire()
        {
            Particle particle = CreateParticle();

                AddParticle(particle);

                int pos = particles.IndexOf(particle);
                var name = "ParticleWillie" + pos.ToString();
                Screen.scriptingEngine.Add(Name, particle);

                IAction moveloop = new RepeatAction(-1);
                TimeSpan time = Time.ms(400);
                switch (pos%4)
                {
                    case 0:
                        moveloop.AddAction2(
                            new MoveAction(name, new Vector2(200, 200), time),
                            true);
                        moveloop.AddAction2(
                            new MoveAction(name, new Vector2(200, -200), time),
                            true);
                        break;
                    case 1:
                        moveloop.AddAction2(
                            new MoveAction(name, new Vector2(-200, -200), time),
                            true);
                        break;
                    case 2:
                        moveloop.AddAction2(
                            new MoveAction(name, new Vector2(-200, 200), time),
                            true);
                        break;
                    case 3:
                        moveloop.AddAction2(
                            new MoveAction(name, new Vector2(200, -200), time),
                            true);
                        break;
                }

                moveloop.Reset();
                Screen.scriptingEngine.AddAction(name, moveloop);

            particle.TimeToLive = 5;
            particle.X = X;
            particle.Y = Y;
            particle.Scale = RNG.Next(0, 20) / 60f;
            particle.Color = new Color(RNG.Next(10, 50), RNG.Next(3, 8), RNG.Next(128, 255));
        }

        public void Fire2()
        {
            Particle particle = CreateParticle();

            particle.Scale = .1f;
            AddParticle(particle);
          
            int pos = particles.IndexOf(particle);
            var name = Name + "-Particle-" + pos;
            Screen.scriptingEngine.Add(name, particle);

            IAction action = new MoveAction(name, new Vector2(0, -1000), 5f);
            Screen.scriptingEngine.AddAction(name, action);

            particle.TimeToLive = 10;
            particle.X = X;
            particle.Y = Y;
        }

        public virtual void AddParticle(Particle p)
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

        public override void Update(GameTime gameTime)
        {
            foreach (var p in particles.AsParallel().Where(p => p.Enabled))
            {
                p.Update(gameTime);

                if (p.TimeToLive > 0 && p.Age > p.TimeToLive)
                {
                    p.Enabled = false;
                    particleBuffer.Add(p);

                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var p in particles.AsParallel().Where(p => p.Enabled))
                p.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
