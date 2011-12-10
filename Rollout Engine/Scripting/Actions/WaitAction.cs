using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{

    [Action("wait")]
    [ActionParam(0, "duration", typeof(int))]
    public class WaitAction : Action
    {
        private TimeSpan waitTime;
        private TimeSpan currentTime;

        public WaitAction(string target, int duration) : base (true)
        {
            waitTime = Time.ms(duration);

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