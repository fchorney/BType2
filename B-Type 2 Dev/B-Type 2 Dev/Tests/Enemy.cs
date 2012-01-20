using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace B_Type_2_Dev
{
    [Sprite("enemy")]
    public class Enemy : Sprite, IFireable
    {
        private EnemyGun Gun;

        public Enemy()
        {
            AddAnimation("main", Animation.Load("bullet"));
            Name = "enemy" + Guid.NewGuid();
            Primary = true;

            Rotation = MathHelper.Pi;
            Shape = new Rectangle(0, 0, 16, 16);


            Gun = new EnemyGun();
            Add(Gun);
        }

        public void Fire()
        {
            Gun.Fire();
        }
    }

    public class EnemyGun : ParticlePool<EnemyBullet>, IFireable
    {
        public EnemyGun() : base()
        {
        }

        public void Fire()
        {

            var bullet = GetParticle();

            //reset bullet state
            bullet.Reset();
            ScriptingEngine.Engine.ResetActionQueue(bullet.Name);

            bullet.X = X;
            bullet.Y = Y;

            bullet.TimeToLive = 25;

            bullet.Enabled = true;

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

            var action = new MoveAction(this.Name, 0, 2000, 0.5f, 0);

            ScriptingEngine.Add(this.Name, this);
            ScriptingEngine.AddAction(this.Name, action);

            CollisionEngine.Add(this);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Rotation += (float)(2 * gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

}