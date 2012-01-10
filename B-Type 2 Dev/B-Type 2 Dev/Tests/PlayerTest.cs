using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Core;
using Rollout.Drawing.Particle;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class PlayerTest : Screen
    {
        private PlayerInput input;

        private Sprite player;
        private bool fireRight = true;
        private Limiter fireLimit;

        private double elapsedTime;
        private Sprite leftGun;
        private ParticleEmitter leftEmitter;
        private Sprite rightGun;
        private ParticleEmitter rightEmitter;

        public override void Initialize()
        {
            // Must happen at the start
            base.Initialize();

            AnimationLoader.Test();

            player = new Sprite(new Vector2(200, 200), Animation.Load("player")){Name = "player"};
            leftGun = new Sprite(new Vector2(-21, 20),
                                 new Animation(@"Sprites/gun1", 32, 32, 2, new double[] {0.05f, 0.08f}, false)){Name = "leftgun"};
            rightGun = new Sprite(new Vector2(57, 20),
                                  new Animation(@"Sprites/gun1", 32, 32, 2, new double[] {0.05f, 0.08f}, false)){Name = "rightgun"};
            leftEmitter = new ParticleEmitter("left-emitter", new Animation(@"Sprites/Lensflare", 16, 16), null, 200)
            {
                OffsetX = 6,
                OffsetY = -18
            };
            rightEmitter = new ParticleEmitter("right-emitter", new Animation(@"Sprites/Lensflare", 16, 16), null, 200)
            {
                OffsetX = 6,
                OffsetY = -18
            };

            // Add in top to bottom graph form
            Add(player);
            player.Add(leftGun);
            player.Add(rightGun);
            leftGun.Add(leftEmitter);
            rightGun.Add(rightEmitter);
          
            //player.AddSprite("Shadow", new Sprite(new Vector2(2,2), new Animation(@"Sprites/spaceship-shadow", 64, 64)), 3);

            input = new PlayerInput(PlayerIndex.One);
            input.BindAction("Quit", Keys.Q);
            input.BindAction("Left", Keys.Left);
            input.BindAction("Right",Keys.Right);
            input.BindAction("Up",Keys.Up);
            input.BindAction("Down",Keys.Down);
            input.BindAction("Fire",Keys.Space);

            fireLimit = new Limiter(.05f);
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
                    rightGun.ReStart();
                    rightEmitter.Emit();
                }
                else
                {
                    leftGun.ReStart();
                    leftEmitter.Emit();
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
