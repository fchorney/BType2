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
    public class Player : Sprite
    {
        public Dictionary<string, PlayerGun> Guns { get; set; }

        public Player()
        {
            Primary = true;

            Position = new Vector2(500,600);
            AddAnimation("main", Animation.Load("player"));
            Name = "player";
            //Shape = new Rectangle(0, 0, 64, 64);
            Shape = new Circle(10,10,8);

            Guns = new Dictionary<string, PlayerGun>();

            var leftGun = new PlayerGun();
            leftGun.Position = new Vector2(8, 28);
            Add(leftGun);
            Guns.Add("left", leftGun);

            var rightGun = new PlayerGun();
            leftGun.Position = new Vector2(48, 28);
            Add(rightGun);
            Guns.Add("right", rightGun);

        }

    }

    public class PlayerGun : ParticlePool<PlayerBullet>, IFireable
    {
        public PlayerGun() : base()
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

            bullet.TimeToLive = 2;

            bullet.Enabled = true;

        }
    }

    public class PlayerBullet : Particle
    {
        public PlayerBullet()
        {
            Name = "PlayerBullet_" + GetHashCode().ToString();

            AddAnimation("main", Animation.Load("bullet"));
            Color = Color.LightCyan;

            Shape = new Circle(0, 0, 8);

            var action = new MoveAction(Name, 0, -2000, 10f, "0");

            ScriptingEngine.Add(Name, this);
            ScriptingEngine.AddAction(Name, action);

            CollisionEngine.Add(this);

        }
    }

}