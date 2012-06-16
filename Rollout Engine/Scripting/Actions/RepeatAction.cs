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
            int n = Args["count"].AsInt();
            if (n == 0) n = -1;
            Iterations = n;
            CurrentIterations = n;
        }


        public RepeatAction(string target, int n)
        {
            if(n==0) n=-1;
            Iterations = n;
            CurrentIterations = n;
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