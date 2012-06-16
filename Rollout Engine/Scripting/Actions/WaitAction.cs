using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Utility;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{

    [Action("wait")]
    [ActionParam("duration")]
    public sealed class WaitAction : Action
    {
        private TimeSpan waitTime;
        private TimeSpan currentTime;
        private Equation rpn;

        public WaitAction(Dictionary<string, Expression> args)
            : base(args)
        {
            Reset();
        }

        public WaitAction(string target, string duration) : base (true)
        {
            rpn = Equation.Parse(duration);
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
            waitTime = Time.ms(Args["duration"].AsInt());
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