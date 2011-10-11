using System;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Drawing;
using Rollout.Utility;
using Point = Rollout.Collision.Point;

namespace Rollout.Scripting.Actions
{
    public class FollowAction : Action, IAction
    {
        const double PixelsInAMeter = 100;


        private Vector2 Delta;
        private double TurningRadius { get; set; }
        private double AngleToTarget { get; set; }
        private double Direction { get; set; }

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

            TurningRadius = Math.PI;
            
            Direction = MathUtil.AngleTo(new Point(Target.X, Target.Y), new Point(FollowTarget.X, FollowTarget.Y));
            AngleToTarget = Direction;

            TextWriter.Add("Target position");
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
            AngleToTarget = MathHelper.WrapAngle((float)MathUtil.AngleTo(new Point(Target.X, Target.Y), new Point(FollowTarget.X, FollowTarget.Y)));
            Direction = MathHelper.WrapAngle((float)Direction);

            double diff = AngleToTarget + Direction;

            TextWriter.Update("Target position", diff.ToString());

            if (Math.Abs(diff) > TurningRadius)
            Direction += TurningRadius * gameTime.ElapsedGameTime.TotalSeconds * (diff < 0 ? 1 : -1);

            float deltaX = (float)Math.Cos(Direction);
            float deltaY = (float)Math.Sin(Direction);

            float speed = (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds * PixelsInAMeter);
            Delta = new Vector2(deltaX * speed, deltaY * speed);

        }
    }
}