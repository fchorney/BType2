using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Drawing;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace B_Type_2_Dev
{
    [Sprite("enemy")]
    public class Enemy : Sprite, IFireable
    {
        private EnemyGun Gun;

        public Enemy()
        {
            AddAnimation("main", Animation.Load("player"));
            Name = "enemy" + this.GetHashCode().ToString();

            Rotation = MathHelper.Pi;
            Shape = new Rectangle(0, 0, 64, 64);


            Gun = new EnemyGun(this);
            Add(Gun);
        }

        public void Fire()
        {
            Gun.Fire();
        }
    }

    public class EnemyGun : ParticlePool<EnemyBullet>, IFireable
    {
        public Sprite Sprite { get; private set; }

        public EnemyGun(Sprite sprite) : base(10)
        {
            Sprite = sprite;
        }

        public void Fire()
        {
            var bullet = GetParticle();

            //reset bullet state
            bullet.Reset();

            bullet.X = Sprite.X;
            bullet.Y = Sprite.Y;

            bullet.Enabled = true;

            var action = new MoveAction(bullet.Name, 0, 10000, 5f, 0);
            ScriptingEngine.Add(bullet.Name, bullet);
            ScriptingEngine.AddAction(bullet.Name, action);
        }
    }

    public class EnemyBullet : Particle
    {
        public EnemyBullet()
        {
            Name = "EnemyBullet_" + this.GetHashCode().ToString();

            AddAnimation("main", new Animation(@"Sprites/Lensflare", 16, 16));

            Shape = new Circle(0, 0, 8);

            //CollisionEngine.Add(this);
        }
    }

}