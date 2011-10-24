using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing;

namespace Rollout.Scripting.Actions
{
    public class MoveAction : Action, IAction
    {
        private Vector2 TargetDelta;
        private Vector2 TotalDelta;
        private Vector2 DeltaRate;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private ITransformable Target { get; set; }
        
        public MoveAction(DateTime startTime,ITransformable targetName, Vector2 delta, TimeSpan duration)
        {
         
            Duration = duration;
            TargetDelta = delta;

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Target = targetName;

            Reset();
        }

        public void Reset()
        {
            base.Reset();
            ElapsedTime = new TimeSpan();
            TotalDelta = new Vector2(0f, 0f);
        }

        public void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime < Duration)
            {

                float dX = DeltaRate.X * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                float dY = DeltaRate.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds);

                TotalDelta.X += dX;
                TotalDelta.Y += dY;

                Target.X += dX;
                Target.Y += dY;

            }
            else
            {
                
                Target.X += TargetDelta.X - TotalDelta.X;
                Target.Y += TargetDelta.Y - TotalDelta.Y;

                Finished = true;
            }

        }
    }
}