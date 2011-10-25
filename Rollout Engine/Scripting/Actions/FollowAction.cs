using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    public class FollowAction : Action, IAction
    {
        const double PixelsInAMeter = 100;

        private Vector2 Delta;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private double Speed { get; set; }

        private ITransformable Target { get; set; }
        private ITransformable FollowTarget { get; set; }

        public FollowAction(String targetName,String followName, double speed)
        {

            Target = Engine[targetName] as ITransformable;
            FollowTarget = Engine[followName] as ITransformable;

            Speed = speed;
        }

        public void Reset()
        {
            base.Reset();
        }

        public void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime;

            Calculate(gameTime);

            Target.X += Delta.X;
            Target.Y += Delta.Y;

        }

        private void Calculate(GameTime gameTime)
        {
            double deltaX = (FollowTarget.X - Target.X) / Speed * gameTime.ElapsedGameTime.TotalSeconds;
            double deltaY = (FollowTarget.Y - Target.Y) / Speed * gameTime.ElapsedGameTime.TotalSeconds;

            Delta = new Vector2((float)deltaX, (float)deltaY);

        }
    }
}