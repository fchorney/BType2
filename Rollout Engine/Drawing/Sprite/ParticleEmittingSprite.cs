using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Scripting;
using Rollout.Scripting.Actions;

namespace Rollout.Drawing
{
    public class ParticleEmittingSprite : Sprite
    {
        public ParticleEmitter Emitter;

        public ParticleEmittingSprite(int bufferSize, string name, Vector2 startPosition, Animation animation = null, String animationName = "main") : 
            base(startPosition,animation,animationName)
        {
            Name = name;
            Emitter = new ParticleEmitter(bufferSize){Name = Name + "-Emitter"};
        }

        public void Fire()
        {
            Emitter.fire2();
        }

        public override void Update(GameTime gameTime)
        {
            //Emitter.X = X;
            //Emitter.Y = Y;
            Emitter.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(ITransformable wrt = null)
        {
            Emitter.Draw();
            base.Draw(wrt);
        }

    }
}
