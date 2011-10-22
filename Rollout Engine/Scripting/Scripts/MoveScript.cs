using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing;

namespace Rollout.Scripting.Scripts
{
    public class MoveScript : Script
    {
        private Vector2 TargetDelta;
        private Vector2 TotalDelta;
        private Vector2 DeltaRate;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private ITransformable Target { get; set; }
        
        public MoveScript(DateTime startTime,ITransformable targetName, Vector2 delta, TimeSpan duration) : base(startTime)
        {
            
            ElapsedTime = new TimeSpan();
            Duration = duration;
            TargetDelta = delta;

            TotalDelta = new Vector2(0f, 0f);
            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

            Target = targetName;
        }

        private Boolean Enabled = true;

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime < Duration)
            {
                var progress = (float)(ElapsedTime.TotalSeconds/Duration.TotalSeconds);

                float dX = DeltaRate.X * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                float dY = DeltaRate.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds);

                TotalDelta.X += dX;
                TotalDelta.Y += dY;

                Target.X += dX;
                Target.Y += dY;

            }
            else
            {
                if (Enabled)
                {
                    Target.X += TargetDelta.X - TotalDelta.X;
                    Target.Y += TargetDelta.Y - TotalDelta.Y;
                    Enabled = false;
                }
            }

        }
    }
}