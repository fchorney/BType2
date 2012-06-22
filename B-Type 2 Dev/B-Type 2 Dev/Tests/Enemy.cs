using System;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
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
            Name = "enemy" + Guid.NewGuid();
            Primary = true;

            Rotation = MathHelper.Pi;
            Shape = new Rectangle(0, 0, 64, 64);

            Gun = new EnemyGun() {OffsetX = 23, OffsetY = 23};
            Add(Gun);
        }

        public void Fire()
        {
            Gun.Fire();
        }
    }

    public class EnemyGun : ParticlePool<EnemyBullet>, IFireable
    {
        private int speed = 250;
        private int count = 0;

        public void Fire()
        {
            var bullet = GetParticle();

            //reset bullet state
            bullet.Reset();
            bullet.Enabled = true;
            bullet.Position = new Vector2(X, Y);
            bullet.TimeToLive = 10;

            count++;


            speed += 25;

            var action = new MoveAction(bullet.Name, "player", "0", speed.ToString());
            ScriptingEngine.AddAction(bullet.Name, action);

            if (count >= 10)
            {
                count = 0;
                speed = 250;
            }

        }
    }

    public class EnemyBullet : Particle
    {
        public EnemyBullet()
        {
            Name = "EnemyBullet_" + Guid.NewGuid();
            AddAnimation("main", Animation.Load("bullet"));
            Shape = new Circle(0, 0, 8);
            Scale = 1.5f;

            ScriptingEngine.Add(Name, this);
            CollisionEngine.Add(this);
        }

        public new void Reset()
        {
            base.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Rotation += (float)(2 * gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

}