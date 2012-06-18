using System;
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
        public EnemyGun() : base(10)
        {
        }

        public void Fire()
        {
            var bullet = GetParticle();

            //reset bullet state
            bullet.Reset();
            ScriptingEngine.Engine.ClearActionQueue(bullet.Name);            


            /**
             * CRAZY RESET JUNK
             **/
            bullet.Initialize();
            bullet.Enabled = true;
            bullet.X = X;
            bullet.Y = Y;
            bullet.Shape.X = X;
            bullet.Shape.Y = Y;
            bullet.TimeToLive = 10;
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

        public override void Initialize()
        {
            base.Initialize();

            //var attackVector = GetAttackVector();
            var action = new MoveAction(Name,"player","0","150");
            ScriptingEngine.AddAction(Name, action);
        }

        private Vector2 GetAttackVector()
        {
            var v = new Vector2();
            ITransformable target = ScriptingEngine.Item("player");

            v.X = target.X - X;
            v.Y = target.Y - Y;

            return v;
        }
       

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rotation += (float)(2 * gameTime.ElapsedGameTime.TotalSeconds);
        }
    }

}