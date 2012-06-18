using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{

    [Action("repeat")]
    [ActionParam("count")]
    public sealed class RepeatAction : Action
    {
        private int Iterations { get; set; }
        private int CurrentIterations { get; set; }

        public RepeatAction(Dictionary<string, Expression> args)
            : base(args)
        {
            int count = Args["count"].AsInt();
            if (count == 0) count = -1;
            Iterations = count;
            CurrentIterations = count;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Actions.Queue.Count == 0)
            {
                if (CurrentIterations == 1)
                {
                    Finished = true;
                    CurrentIterations = Iterations;
                }
                else
                {
                    CurrentIterations--;
                    Reset();
                }
            }
        }
    }
}