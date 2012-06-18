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
        public FireAction(Dictionary<string, Expression> args)
            : base(args)
        {
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var shooter = Source as IFireable;
            if (shooter != null) 
                shooter.Fire();
            Finished = true;
        }
    }
}
