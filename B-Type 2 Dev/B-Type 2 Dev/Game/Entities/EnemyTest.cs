using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Collision.Shapes;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace Rollout.Game.Entities
{
    public class EnemyTest : Sprite
    {
        private ParticleEmitter gun;
        public EnemyTest(string name = "enemy")
        {
            AddAnimation("main", Animation.Load("player"));
            Name = name;

            Position = Vector2.Zero;
            Rotation = MathHelper.Pi;
            Shape = new Rectangle(0, 0, 64, 64);

            gun = new ParticleEmitter(name + "-emitter", new Animation(@"Sprites/Lensflare", 16, 16), null, 30, new Circle(0,0,8));
            Add(gun);
        }

        public void Fire()
        {
            gun.Emit(3);
        }
    }
}
