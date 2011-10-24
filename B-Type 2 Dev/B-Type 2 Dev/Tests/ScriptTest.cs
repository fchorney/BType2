using System;
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
        private Sprite player { get; set; }
        private IScriptingEngine scriptingEngine { get; set; }
        private PlayerInput input { get; set; }

        public ScriptTest() : base(G.Game)
        {

        }

        public override void Initialize()
        {
            player = new Sprite(new Vector2(400, 200)){ Name = "BigWilly" }; 
            player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));

            IAction moveloop = new RepeatAction(-1);

            moveloop.Actions.Add(new MoveAction(DateTime.Now, player, new Vector2(200,200), new TimeSpan(0,0,0,0, 50)));
            moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 50)));
            moveloop.Actions.Add(new MoveAction(DateTime.Now, player, new Vector2(-200,200), new TimeSpan(0,0,0,0,30)));
            moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 30)));
            moveloop.Actions.Add(new MoveAction(DateTime.Now, player, new Vector2(-200,-200), new TimeSpan(0,0,0,0,20)));
            moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 20)));
            moveloop.Actions.Add(new MoveAction(DateTime.Now, player, new Vector2(200,-200), new TimeSpan(0,0,0,0,50)));
            moveloop.Actions.Add(new WaitAction(new TimeSpan(0, 0, 0, 0, 50)));
            



            moveloop.Reset();

            player.Actions.Add(new MoveAction(DateTime.Now, player, new Vector2(200, 200), new TimeSpan(0, 0, 0, 5, 50)));

            scriptingEngine = new ScriptingEngine();
            scriptingEngine.Add(player);

            TextWriter.Add("Target position");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            scriptingEngine.Update(gameTime);

            player.Update(gameTime);
            player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            //player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            TextWriter.Update("Target position", "[" + player.X + ", " + player.Y + "]");

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
