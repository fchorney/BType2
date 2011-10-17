using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Drawing.Examples;
using Rollout.Input;
using Rollout.Utility;

namespace B_Type_2_Dev
{
    public class PlayerTest : DrawableGameComponent
    {
        private PlayerInput input;
        //private Sprite player;

        private SpriteComponent player;

        public PlayerTest()
            : base(G.Game)
        {
        }

        public override void Initialize()
        {
            //player = new Sprite(new Vector2(600, 320f));
            //player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
           // player.AddAnimation("alternate", new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { 0.1f, 0.2f }));
            //player.Animation.Loop = false;

            //player.AddChild(new SubSprite(new Vector2(2,2), "main", new Animation(@"Sprites/spaceship-shadow", 64, 64, 1, new double[] {1.0f})));
            //player.AddChild(new SubSprite(new Vector2(-21, 20), "main", new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { .05f, .08f }), true));
            //player.AddChild(new SubSprite(new Vector2(57, 20), "main", new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { .08f, .05f }), true));

            player = new SpriteComponent();
            player.AddSprite("main", new Sprite(new Vector2(200, 200), new Animation(@"Sprites/spaceship2", 64 ,64, 2, new double[] {0.1f, 0.2f})), 4, true);
            player.AddSprite("LeftGun", new Sprite(new Vector2(-21, 20), new Animation(@"Sprites/gun1", 32, 32, 2, new double[] {0.05f, 0.08f})), 5);
            player.AddSprite("RightGun", new Sprite(new Vector2(57, 20), new Animation(@"Sprites/gun1", 32, 32, 2, new double[] { 0.05f, 0.08f })), 5);
            player.AddSprite("Shadow", new Sprite(new Vector2(2,2), new Animation(@"Sprites/spaceship-shadow", 64, 64)), 3);
            
            input = new PlayerInput(PlayerIndex.One);
            G.Game.Components.Add(input);

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

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            player.Update(gameTime);
            //player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            //player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            //if (input.IsPressed("Pause"))
            //    player.Pause();

            if (input.IsPressed("Quit"))
                G.Game.Exit();

            //if (input.IsPressed("UnPause"))
            //    player.UnPause();

            //if (input.IsPressed("Switch-A"))
            //    player.SetAnimation("main");

            //if (input.IsPressed("Switch-S"))
            //    player.SetAnimation("alternate");

            //if (input.IsPressed("Restart"))
            //    player.ReStart();


            if (input.IsHeld("Left"))
                player.X -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.X += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Y -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Y += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
