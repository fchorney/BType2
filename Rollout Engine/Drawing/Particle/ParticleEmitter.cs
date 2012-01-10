using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Core;
using Rollout.Scripting;
using Rollout.Scripting.Actions;

namespace Rollout.Drawing.Particle
{
    public class ParticleEmitter : DrawableGameObject
    {
        private List<Particle> particles;
        private List<Particle> buffer;

        private Animation particleAnimation;
        private IShape particleShape;
        private CollisionHandler particleHandler;
        private IAction particleAction;
        private string name;

        private ulong particleNumber;

        public int PCount
        {
            get { return particles.Count; }
        }

        public int BCount
        {
            get { return buffer.Count; }
        }

        public ParticleEmitter(string emitterName, Animation anim, IAction action, int bufferSize = 0,
                               IShape shape = null, CollisionHandler handler = null)
        {
            particles = new List<Particle>();
            buffer = new List<Particle>();
            Enabled = true;
            particleAnimation = anim;
            particleShape = shape;
            particleHandler = handler;
            particleAction = action;
            name = emitterName;
            particleNumber = 0;

            for (var i = 0; i < bufferSize; i++)
            {
                buffer.Add(CreateParticle());
            }
        }

        private Particle CreateParticle()
        {
            var p = new Particle(Vector2.Zero, particleAnimation)
                        {
                            Shape = particleShape.DeepCopy(),
                            OnCollision = particleHandler,
                            Enabled = false,
                            Name = name + "-Particle-" + particleNumber++
                        };
            CollisionEngine.Add(p);
            ScriptingEngine.Add(p.Name, p);

            //DEBUG MUST BE REPLACED LATER
            particleAction = new MoveAction(p.Name, new Vector2(0, -100000), 10f);
            /***********************************/

            ScriptingEngine.AddAction(p.Name, particleAction);
            return p;
        }

        private void ResetParticle(Particle p)
        {
            p.Initialize();
            p.Position = Vector2.Zero;
            ScriptingEngine.Engine.ResetActionQueue(p.Name);
            p.Enabled = true;
        }

        private Particle GetParticle()
        {
            Particle p;

            if (buffer.Count > 0)
            {
                p = buffer[0];
                buffer.RemoveAt(0);
                ResetParticle(p);
            }
            else
            {
                p = CreateParticle();
            }

            return p;
        }

        public void Emit(int timeToLive = 5)
        {
            var p = GetParticle();
            p.TimeToLive = timeToLive;
            p.X = X;
            p.Y = Y;
            AddParticle(p);
        }

        public void AddParticle(Particle p)
        {
            var index = particles.IndexOf(p);
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

        public override void Update(GameTime gameTime)
        {
            foreach (var p in particles.Where(p => p.Enabled))
            {
                p.Update(gameTime);

                if (p.TimeToLive > 0 && p.Age > p.TimeToLive)
                {
                    p.Enabled = false;
                    buffer.Add(p);
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
