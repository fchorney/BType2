using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Screens;
using Rollout.Scripting;

namespace Rollout.Tests
{
    class XMLTest : Screen
    {
        private ScriptProvider scriptProvider;
        private Sprite sprite;
        private DrawableGameObject enemies;


        public override void Initialize()
        {
            sprite = new Sprite(new Vector2(200, 200),
                                new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { 0.3f, 0.3f })) { Name = "TheBiggest" };
            Add(sprite);
            scriptingEngine.Add(sprite.Name, sprite);




            enemies = new DrawableGameObject();
            Add(enemies);
            scriptingEngine.Add("enemies", enemies);


            scriptProvider = new ScriptProvider(scriptingEngine);
            scriptProvider.Load("MoveTest1");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();
            base.Draw(gameTime);
            G.SpriteBatch.End();      
        }
    }
}
