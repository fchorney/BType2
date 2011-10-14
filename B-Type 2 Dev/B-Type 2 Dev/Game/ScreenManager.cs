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
using Rollout.Scripting.Actions;
using Rollout.Scripting.Scripts;
using Rollout.Utility;
using Action = Rollout.Scripting.Action;

namespace B_Type_2_Dev
{
    public class ScreenManager : Screen
    {

        private PlayerInput input;
        private Dictionary<string, Screen> screens;

        public override void Initialize()
        {

            base.Initialize();

            input = new PlayerInput(PlayerIndex.One);

            input.BindAction("Exit", Keys.Z);
            input.BindAction("Enter", Keys.X);

            screens = new Dictionary<string, Screen>();
            
            screens.Add("scripting", new ScriptTest());
            //screens.Add("particles", new ParticlesTest());
            screens.Add("playertest", new PlayerTest());

            foreach (var screen in screens.Values)
            {
                this.Add(screen);
            }
            
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
                this.Add(screens["playertest"]);
            }

        }

    }
}
