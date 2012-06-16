using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Scripting;
using Rollout.Utility.EquationHelper;
using Action = Rollout.Scripting.Actions.Action;

namespace B_Type_2_Dev
{
    public interface IFireable
    {
        void Fire();
    }

    [Action("fire")]
    public sealed class FireAction : Action
    {
        private string target;

        public FireAction(Dictionary<string, Expression> args)
            : base(args)
        {
            target = Args["source"].AsString();
            Reset();
        }



        public FireAction(string target)
        {
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var shooter = ScriptingEngine.Item(target) as IFireable;
            if (shooter != null) 
                shooter.Fire();
            Finished = true;
        }
    }
}
