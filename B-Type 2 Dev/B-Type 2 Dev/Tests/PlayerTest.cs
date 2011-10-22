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
        private Sprite player;

        public PlayerTest()
            : base(G.Game)
        {
        }

        public override void Initialize()
        {
            player = new Sprite(new Vector2(600, 320f));
            player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
            player.AddAnimation("alternate", new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { 0.1f, 0.2f }));
            //player.Animation.Loop = false;

            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Pause", Keys.P);
            input.BindAction("UnPause", Keys.U);
            input.BindAction("Switch-A", Keys.A);
            input.BindAction("Switch-S", Keys.S);
            input.BindAction("Restart", Keys.R);
            input.BindAction("Quit", Keys.Q);


            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            player.Update(gameTime);
            player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            if (input.IsPressed("Pause"))
                player.Pause();

            if (input.IsPressed("Quit"))
                G.Game.Exit();

            if (input.IsPressed("UnPause"))
                player.UnPause();

            if (input.IsPressed("Switch-A"))
                player.SetAnimation("main");

            if (input.IsPressed("Switch-S"))
                player.SetAnimation("alternate");

            if (input.IsPressed("Restart"))
                player.ReStart();
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
