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
using Rollout.Scripting.Scripts;
using Rollout.Utility;

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
            player = new Sprite(new Vector2(200, 200)){ Name = "BigWilly" }; 
            player.AddAnimation("main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { 0.5f, 1.0f }, true, 5));

            input = new PlayerInput(PlayerIndex.One);
            input.BindAction("Move", Keys.A);
            input.BindAction("MoveBack", Keys.S);

            scriptingEngine = new ScriptingEngine();
            scriptingEngine.Add(player);

            TextWriter.Add("Target position");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.IsPressed("Move"))
            {
                IScript move = new MoveScript(DateTime.Now, player, new Vector2(500, 300), new TimeSpan(0, 0, 1));
                scriptingEngine.AddScript(move);
            }

            if (input.IsPressed("MoveBack"))
            {
                IScript move = new MoveScript(DateTime.Now, player, new Vector2(-500, -300), new TimeSpan(0,0,0,0,100));
                scriptingEngine.AddScript(move);
            }

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
