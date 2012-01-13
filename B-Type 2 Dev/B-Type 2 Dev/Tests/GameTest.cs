using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rollout.Core.GameObject;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace B_Type_2_Dev
{

    public class Enemy : Sprite
    {
        public Enemy()
        {
            AddAnimation("main", Animation.Load("player"));
            Name = "enemy";

            Position = new Vector2(300,200);
            Rotation = MathHelper.Pi;
            Shape = new Rectangle(0, 0, 64, 64);

        }
    }

    public class Player : Sprite
    {
        public Dictionary<string, Gun> Guns { get; set; }

        public Player(Screen screen)
        {
            Position = new Vector2(500,600);
            AddAnimation("main", Animation.Load("player"));
            Name = "player";
            Shape = new Rectangle(0, 0, 64, 64);

            Guns = new Dictionary<string, Gun>();
            Guns.Add("left", new Gun(screen,"left", new Vector2(-21, 20), new Vector2(6, -18)));
            Guns.Add("right", new Gun(screen,"right", new Vector2(57, 20), new Vector2(6, -18)));

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

        public Gun(Screen screen, string name, Vector2 gunOffset, Vector2 emitterOffset)
        {
            Sprite = new Sprite(gunOffset, new Animation(@"Sprites/gun1", 32, 32, 2, new double[] {0.05f, 0.08f}, false))
                         {Name = name};

            CollisionHandler handler = (src, obj) => {  };

            Emitter = new ParticleEmitter(name + "-emitter", new Animation(@"Sprites/Lensflare", 16, 16), null, 200, new Circle(0,0,8), handler)
            {
                OffsetX = emitterOffset.X,
                OffsetY = emitterOffset.Y
            };

            Sprite.Add(Emitter);
        }

        public void Fire()
        {
            Sprite.ReStart();
            Emitter.Emit(2);   
        }
    }

    public class GameTest : Screen
    {
        private PlayerInput input;

        private Player player;
        private bool fireRight = true;
        private Limiter fireLimit;

        private double elapsedTime;

        private Enemy enemy;

        public void GetHitByABullet(Sprite s, Particle p)
        {
            s.Rotation += 0.1f;
        }

        public void GetHitByASprite(Sprite s, Sprite p)
        {
            s.Rotation += 0.1f;
            p.Rotation += 0.1f;
        }

        public override void Initialize()
        {
            // Must happen at the start
            base.Initialize();
            //CollisionEngine.Debug = true;

            //CollisionEngine.Register<Enemy, Particle>(GetHitByABullet);
            CollisionEngine.Register<Sprite, Particle>(GetHitByABullet);
            CollisionEngine.Register<Player, Sprite>(GetHitByASprite);
            CollisionEngine.Register<Player, Enemy>(GetHitByASprite);

            AnimationLoader.Test();

            enemy = new Enemy();
            Add(enemy);

            player = new Player(this);
            Add(player);
          
            input = new PlayerInput(PlayerIndex.One);
            input.BindAction("Quit", Keys.Q);
            input.BindAction("Left", Keys.Left);
            input.BindAction("Right",Keys.Right);
            input.BindAction("Up",Keys.Up);
            input.BindAction("Down",Keys.Down);
            input.BindAction("Fire",Keys.Space);

            fireLimit = new Limiter(.05f);

            CollisionEngine.Add(player);
            CollisionEngine.Add(enemy);


            var enemies = new DrawableGameObject();
            Add(enemies);
            ScriptingEngine.Add("enemies", enemies);

            var scriptProvider = new ScriptProvider(ScriptingEngine.Engine as XmlScriptingEngine);
            scriptProvider.Load("GameTest");

        }

        public override void Update(GameTime gameTime)
        {
            int speed = 500;
            fireLimit.Update(gameTime);

            if (input.IsPressed("Quit"))
                G.Game.Exit();

            if (input.IsHeld("Left"))
                player.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (input.IsPressed("Fire"))
            {
                elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
                FireGun();
            }

            if (input.IsReleased("Fire"))
            {
                elapsedTime = 0;
            }

            if (input.IsHeld("Fire"))
            {
                elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (elapsedTime > 0.2f)
                {
                    FireGun();
                }
            }

            base.Update(gameTime);
        }

        private void FireGun()
        {
            if (fireLimit.Ready)
            {
                 if (fireRight)
                {
                    player.Guns["right"].Fire();
                }
                else
                {
                    player.Guns["left"].Fire();
                }
                fireRight = !fireRight;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin(Transition.Transform());
            base.Draw(gameTime);
            G.SpriteBatch.End();            
        }
    }
}
