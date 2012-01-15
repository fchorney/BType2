using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rollout.Core.GameObject;
using Rollout.Drawing;
using Rollout.Drawing.Particles;
using Rollout.Drawing.Sprites;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Utility;

namespace B_Type_2_Dev
{

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
            CollisionEngine.Debug = true;

            CollisionEngine.Register<Enemy, PlayerBullet>(GetHitByABullet);
            CollisionEngine.Register<Player, EnemyBullet>(GetHitByABullet);

            CollisionEngine.Register<Player, Enemy>(GetHitByASprite);

            AnimationLoader.Test();

            player = new Player();
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
