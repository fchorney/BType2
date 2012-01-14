using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace B_Type_2_Dev
{
    public class Player : Sprite
    {
        public Dictionary<string, Gun> Guns { get; set; }

        public Player()
        {
            Position = new Vector2(500,600);
            AddAnimation("main", Animation.Load("player"));
            Name = "player";
            Shape = new Rectangle(0, 0, 64, 64);

            Guns = new Dictionary<string, Gun>();
            Guns.Add("left", new Gun("left", new Vector2(-21, 20), new Vector2(6, -18)));
            Guns.Add("right", new Gun("right", new Vector2(57, 20), new Vector2(6, -18)));

            foreach (var gun in Guns.Values)
            {
                Add(gun.Sprite);
            }
        }

    }

    public class Gun
    {
        public Sprite Sprite { get; set; }
        public ParticleEmitter Emitter { get; set; }

        public Gun(string name, Vector2 gunOffset, Vector2 emitterOffset)
        {
            Sprite = new Sprite(gunOffset, new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { 0.05f, 0.08f }, false)) { Name = name };

            CollisionHandler handler = (src, obj) => { };

            Emitter = new ParticleEmitter(name + "-emitter", new Animation(@"Sprites/Lensflare", 16, 16), null, 0, new Circle(0, 0, 8), handler)
            {
                OffsetX = emitterOffset.X,
                OffsetY = emitterOffset.Y
            };

            Sprite.Add(Emitter);
        }

        public void Fire()
        {
            Sprite.ReStart();
            Emitter.Emit(5);
        }
    }
}