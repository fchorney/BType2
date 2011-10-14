using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Scripting;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class PlayerTest : Screen
    {
        private PlayerInput input;

        private SpriteComponent player;
        private bool fireRight = true;
        private Limiter fireLimit;

        private double elapsedTime;


        public override void Initialize()
        {
            player = new SpriteComponent();
            player.AddSprite("main", new Sprite(new Vector2(200, 200), new Animation(@"Sprites/spaceship2", 64 ,64, 2, new double[] {0.1f, 0.2f})), 4, true);
            player.AddSprite("LeftGun", new ParticleEmittingSprite(200, "LeftGun", new Vector2(-21, 20), new Animation(@"Sprites/gun1", 32, 32, 2, new double[] {0.05f, 0.08f}, false)), 5);
            player.AddSprite("RightGun", new ParticleEmittingSprite(200, "RightGun", new Vector2(57, 20), new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { 0.05f, 0.08f }, false)), 5);
            //Add to scripting Engine
            ScriptingEngine.Instance.Add(player["LeftGun"]);
            ScriptingEngine.Instance.Add(player["RightGun"]);
            
            player.AddSprite("Shadow", new Sprite(new Vector2(2,2), new Animation(@"Sprites/spaceship-shadow", 64, 64)), 3);

            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Pause", Keys.P);
            input.BindAction("UnPause", Keys.U);
            input.BindAction("Switch-A", Keys.A);
            input.BindAction("Switch-S", Keys.S);
            input.BindAction("Restart", Keys.R);
            input.BindAction("Quit", Keys.Q);

            input.BindAction("Left", Keys.Left);
            input.BindAction("Right",Keys.Right);
            input.BindAction("Up",Keys.Up);
            input.BindAction("Down",Keys.Down);
            input.BindAction("Fire",Keys.Space);

            fireLimit = new Limiter(.05f);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            player.Update(gameTime);
            fireLimit.Update(gameTime);
            ScriptingEngine.Instance.Update(gameTime);
            //player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            //player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            if (input.IsPressed("Quit"))
                G.Game.Exit();

            if (input.IsHeld("Left"))
                player.X -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.X += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Y -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Y += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            ((ParticleEmittingSprite)player["LeftGun"]).Emitter.X = player.X + player["LeftGun"].X - 120;
            ((ParticleEmittingSprite)player["LeftGun"]).Emitter.Y = player.Y + player["LeftGun"].Y - 150;
            ((ParticleEmittingSprite)player["RightGun"]).Emitter.X = player.X + player["RightGun"].X - 110;
            ((ParticleEmittingSprite)player["RightGun"]).Emitter.Y = player.Y + player["RightGun"].Y - 150;
        }

        private void FireGun()
        {
            if (fireLimit.Ready)
            {
                if (fireRight)
                {
                    player["RightGun"].ReStart();
                    ((ParticleEmittingSprite)player["RightGun"]).Fire();
                }
                else
                {
                    player["LeftGun"].ReStart();
                    ((ParticleEmittingSprite)player["LeftGun"]).Fire();
                }
                fireRight = !fireRight;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();
            player.Draw();
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
