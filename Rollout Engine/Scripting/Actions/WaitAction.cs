using System;
using Microsoft.Xna.Framework;
using Rollout.Utility;
using Rollout.Utility.ShuntingYard;

namespace Rollout.Scripting.Actions
{

    [Action("wait")]
    [ActionParam(0, "duration", typeof(string))]
    public sealed class WaitAction : Action
    {
        private TimeSpan waitTime;
        private TimeSpan currentTime;
        private RPNCalculation rpn;

        public WaitAction(string target, string duration) : base (true)
        {
            rpn = ShuntingYard.Parse(duration);
            waitTime = Time.ms(rpn.SolveAsInt());

            Reset();
        }

        public WaitAction(TimeSpan timeSpan) : base (true)
        {
            waitTime = timeSpan;

            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            waitTime = Time.ms(rpn.SolveAsInt());
            currentTime = new TimeSpan();
        }

        public override void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;

            if(currentTime > waitTime)
            {
                Finished = true;
            }
        }

    }
}