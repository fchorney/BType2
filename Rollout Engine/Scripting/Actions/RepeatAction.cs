using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Actions
{
    public class RepeatAction : Action
    {
        private int Iterations { get; set; }
        private int CurrentIterations { get; set; }


        public RepeatAction(int n)
        {
            Iterations = n;
            CurrentIterations = n;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Actions.Queue.Count == 0)
            {
                if (CurrentIterations == 0)
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