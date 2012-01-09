using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Core;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Rectangle;

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

        public ParticleEmitter (int bufferSize = 0, Screen screen = null)
        {

            particles = new List<Particle>();
            particleBuffer = new List<Particle>();
            Enabled = true;

            for (var i = 0; i < bufferSize; i++)
            {
                particleBuffer.Add(CreateParticle());
                //particleBuffer.Add(new Particle(new Vector2(0, 0), new Animation(@"Sprites/Lensflare", 256, 256)));
            }
        }

        public Particle CreateParticle()
        {
            var p = new Particle(new Vector2(0, 0), new Animation(@"Sprites/Lensflare", 256, 256));
                p.Shape = new Circle(0, 0, 16);
                //p.Shape = new Rectangle(0, 0, 25, 25);
                CollisionEngine.Add(p);
                p.OnCollision = (src, obj) => src.Enabled = false;
                p.Enabled = false;

            return p;
        }

        public Particle GetParticle()
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
                p = CreateParticle();
            }

            return p;
        }

        public void Fire()
        {
            Particle particle = GetParticle();

                AddParticle(particle);

                int pos = particles.IndexOf(particle);
                var name = "ParticleWillie" + pos.ToString();
                ScriptingEngine.Add(Name, particle);

                IAction moveloop = new RepeatAction("",-1);
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
                ScriptingEngine.AddAction(name, moveloop);

            particle.TimeToLive = 5;
            particle.X = X;
            particle.Y = Y;
            particle.Scale = RNG.Next(0, 20) / 60f;
            particle.Color = new Color(RNG.Next(10, 50), RNG.Next(3, 8), RNG.Next(128, 255));
        }

        public void Fire2()
        {
            Particle particle = GetParticle();

            //particle.Scale = .1f;
            AddParticle(particle);
          
            int pos = particles.IndexOf(particle);
            var name = Name + "-Particle-" + pos;
            ScriptingEngine.Add(name, particle);

            IAction action = new MoveAction(name, new Vector2(0, -10000), 10f);
            ScriptingEngine.AddAction(name, action);

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
