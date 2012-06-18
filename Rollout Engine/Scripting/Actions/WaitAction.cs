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
        private TimeSpan WaitTime { get; set; }
        private TimeSpan ElapsedTime { get; set; }

        public WaitAction(Dictionary<string, Expression> args)
            : base(args)
        {
            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            WaitTime = Time.ms(Args["duration"].AsInt());
            ElapsedTime = new TimeSpan();
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime > WaitTime)
            {
                Finished = true;
            }
        }

    }
}