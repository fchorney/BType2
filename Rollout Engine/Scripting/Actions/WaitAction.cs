using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Actions
{
    public class WaitAction : Action, IAction
    {
        private TimeSpan waitTime;
        private TimeSpan currentTime;

        public WaitAction(TimeSpan timeSpan) : base (true)
        {
            waitTime = timeSpan;

            Reset();
        }

        public void Reset()
        {
            base.Reset();
            currentTime = new TimeSpan();
        }

        public void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;
            if(currentTime > waitTime)
            {
                Finished = true;
            }
        }

    }
}