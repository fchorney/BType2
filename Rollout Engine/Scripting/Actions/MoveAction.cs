using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    public class MoveAction : Action, IAction
    {
        const double PixelsInAMeter = 100;

        private Vector2 TargetDelta;
        private Vector2 TotalDelta;
        private Vector2 DeltaRate;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private ITransformable Target { get; set; }

        public MoveAction(String targetName, Vector2 delta, double speed, ScriptingEngine scriptingEngine = null)
        {
            if (scriptingEngine != null)
                engine = scriptingEngine;

            double distance = Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            Duration = Time.ms((int)(distance / speed * 1000 / PixelsInAMeter));
            TargetDelta = delta;

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Target = Engine[targetName] as ITransformable;
        }

        public MoveAction(String targetName, Vector2 delta, TimeSpan duration, ScriptingEngine scriptingEngine = null)
        {
            if (scriptingEngine != null)
                engine = scriptingEngine;

            Duration = duration;
            TargetDelta = delta;

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Target = Engine[targetName] as ITransformable;

            Reset();
        }

        public override void Reset()
        {
            base.Reset();
            ElapsedTime = new TimeSpan();
            TotalDelta = new Vector2(0f, 0f);
        }

        public override void Update(GameTime gameTime)
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