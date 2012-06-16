using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Drawing;
using Rollout.Utility;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{
    [Action("moveto")]
    [ActionParam("x")]
    [ActionParam("y")]
    [ActionParam("speed")]
    [ActionParam("duration")]
    public sealed class MoveToAction : Action
    {
        const double PixelsInAMeter = 100;

        private Vector2 TargetDelta;
        private Vector2 TotalDelta;
        private Vector2 DeltaRate;

        private TimeSpan ElapsedTime { get; set; }
        private TimeSpan Duration { get; set; }

        private string targetName;
        private ITransformable target;

        private ITransformable Target
        {
            get { return target ?? (target = ScriptingEngine.Item(targetName) as ITransformable); }
        }

        public MoveToAction(Dictionary<string, Expression> args) : base(args)
        {
            Reset();
        }

        public MoveToAction(String targetName, int x, string y, double speed, String duration)
        {
            this.targetName = targetName;

            Args.Add("x", new Expression(x.ToString()));
            Args.Add("y", new Expression(y.ToString()));
            Args.Add("speed", new Expression(speed.ToString()));
            Args.Add("duration", new Expression(duration));

            Reset();
        }

        public override void Reset()
        {
            base.Reset();

            int x = Args["x"].AsInt();
            int y = Args["y"].AsInt();
            int speed = Args["speed"].AsInt();
            if (speed > 0)
            {
                double distance = Math.Sqrt(x * x + y * y);
                Duration = Time.ms((int)(distance / speed * 1000f / PixelsInAMeter));
            }
            else
            {
                Duration = Time.ms(Args["duration"].AsInt());
            }

            TargetDelta = new Vector2(x, y);

            DeltaRate = new Vector2((float)(TargetDelta.X / Duration.TotalSeconds), (float)(TargetDelta.Y / Duration.TotalSeconds));

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