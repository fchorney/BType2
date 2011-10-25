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
using Rollout.Scripting;
using Rollout.Scripting.Actions;
using Rollout.Scripting.Scripts;
using Rollout.Utility;
using Action = Rollout.Scripting.Action;

namespace B_Type_2_Dev
{
    public class ScriptTest : DrawableGameComponent
    {
        private IScriptingEngine scriptingEngine { get; set; }
        private PlayerInput input;


        private Sprite player { get; set; }
        private List<Sprite> enemies { get; set; }

        public ScriptTest() : base(G.Game)
        {

        }

        public override void Initialize()
        {
            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Left", Keys.Left);
            input.BindAction("Right", Keys.Right);
            input.BindAction("Up", Keys.Up);
            input.BindAction("Down", Keys.Down);


            scriptingEngine = ScriptingEngine.Instance;


            player = new Sprite(new Vector2(500, 200)) { Name = "TheBiggest"};
            player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
            scriptingEngine.Add(player);


            enemies = new List<Sprite>();

            for (int i = 0; i < 2; i++)
            {
                var enemy = new Sprite(new Vector2(300 + 100 * i, 200)) { Name = "BigWilly" + i.ToString() };
                enemy.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
                enemies.Add(enemy);

                scriptingEngine.Add(enemy);

                IAction moveloop = new RepeatAction(-1);

                if (i % 2 == 0)
                {
                    moveloop.AddAction(new MoveAction(enemy.Name, new Vector2(200, 200), Time.ms(100)), true);
                    moveloop.AddAction(new MoveAction(enemy.Name, new Vector2(-200, -200), Time.ms(100)), true);
                }
                else
                {
                    moveloop.AddAction(new FollowAction(enemy.Name, player.Name, 2.82842712474));
                }

                moveloop.Reset();

                enemy.Actions.Add(moveloop);

            }

            TextWriter.Add("Target position");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            if (input.IsHeld("Left"))
                player.X -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Right"))
                player.X += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Up"))
                player.Y -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.IsHeld("Down"))
                player.Y += 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;


            scriptingEngine.Update(gameTime);

            player.Update(gameTime);
            //player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            //player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            //TextWriter.Update("Target position", "[" + player.X + ", " + player.Y + "]");

        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();

            player.Draw();

            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }
            
            G.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
