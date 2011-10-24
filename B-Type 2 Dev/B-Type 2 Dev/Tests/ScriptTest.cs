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

        private List<Sprite> enemies { get; set; }

        public ScriptTest() : base(G.Game)
        {

        }

        public override void Initialize()
        {
            scriptingEngine = ScriptingEngine.Instance;

            enemies = new List<Sprite>();

            for (int i = 0; i < 2; i++)
            {
                var enemy = new Sprite(new Vector2(300 + 100 * i, 200)) { Name = "BigWilly" + i.ToString() };
                enemy.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));
                enemies.Add(enemy);

                IAction moveloop = new RepeatAction(-1);

                if (i % 2 == 0)
                {
                    moveloop.Actions.Add(new MoveAction(enemy, new Vector2(200, 200), new TimeSpan(0, 0, 0, 0, 100)));
                    moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 100)));
                    moveloop.Actions.Add(new MoveAction(enemy, new Vector2(-200, -200), new TimeSpan(0, 0, 0, 0, 100)));
                    moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 100))); 
                }
                else
                {
                    moveloop.Actions.Add(new MoveAction(enemy, new Vector2(200, 200), 28.2842712474));
                    moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 100)));
                    moveloop.Actions.Add(new MoveAction(enemy, new Vector2(-200, -200), 28.2842712474));
                    moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 100)));
                }



                moveloop.Reset();

                enemy.Actions.Add(moveloop);


                scriptingEngine.Add(enemy);

            }



            



            TextWriter.Add("Target position");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            scriptingEngine.Update(gameTime);

            //player.Update(gameTime);
            //player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            //player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            //TextWriter.Update("Target position", "[" + player.X + ", " + player.Y + "]");

        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();

            foreach (var enemy in enemies)
            {
                enemy.Draw();
            }
            
            G.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
