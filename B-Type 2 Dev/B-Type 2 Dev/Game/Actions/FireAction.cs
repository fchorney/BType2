using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Game.Entities;
using Rollout.Scripting;

namespace Rollout.Game.Actions
{
    [Action("fire")]
    public sealed class FireAction : Scripting.Actions.Action
    {
        private string target;
        public FireAction(string target)
        {
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var shooter = ScriptingEngine.Item(target) as EnemyTest;
            if (shooter != null) shooter.Fire();
            Finished = true;
        }
    }
}
