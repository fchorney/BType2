using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rollout.Input;
using Rollout.Screens;
using Rollout.Tests;

namespace B_Type_2_Dev
{
    public class Manager : ScreenManager
    {
        private PlayerInput input;
        private Dictionary<string, Screen> screens;

        public override void Initialize()
        {
            base.Initialize();

            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Exit", Keys.Z);
            input.BindAction("Enter", Keys.X);

            screens = new Dictionary<string, Screen>
            {
                //{"scripting", new ScriptTest()}
                //{"player", new PlayerTest()},
                //{"particles", new ParticlesTest()}
                //{"xmltest", new XMLTest()}
                {"gametest", new GameTest()}
            };

            foreach (var screen in screens.Values)
                Add(screen);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (input.IsPressed("Exit"))
            {
                screens["particles"].ExitScreen();
            }
            if (input.IsPressed("Enter"))
            {
                Add(screens["particles"]);
            }

        }

    }
}
