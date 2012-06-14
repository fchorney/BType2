using System;
using Microsoft.Xna.Framework;
using Rollout.Drawing;
using Rollout.Utility;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{
    [Action("move")]
    [ActionParam(0, "target", typeof(string))]
    [ActionParam(1, "direction", typeof(string))]
    [ActionParam(2, "speed", typeof(string))]
    public sealed class MoveAction : Action
    {
        private Vector2 Speed;

        private Equation direction, speed;

        private string sourceName, targetName;
        private ITransformable source, target;
        private bool initialized;

        private ITransformable Source
        {
            get { return source ?? (source = ScriptingEngine.Item(sourceName)); }
        }

        private ITransformable Target
        {
            get { return target ?? (target = ScriptingEngine.Item(targetName)); }
        }

        public MoveAction(string source, string target, string direction, string speed)
        {
            sourceName = source;
            targetName = target;

            this.direction = Equation.Parse(direction);
            this.speed = Equation.Parse(speed);

            //Reset();
        }

        public override void Reset()
        {
            base.Reset();

            initialized = true;
            int currSpeed = speed.SolveAsInt();
            //int currDirection = direction.SolveAsInt();

            double dx = Target.X - Source.X;
            double dy = Target.Y - Source.Y;
            double ax = Math.Abs(dx);
            double ay = Math.Abs(dy);

            double ratio = 1 / Math.Max(ax, ay);
            // Crazy Magic Numbers
            ratio = ratio * (1.29289 - (ax + ay) * ratio * 0.29289);

            Speed = new Vector2((float) (currSpeed * dx * ratio), (float) (currSpeed * dy * ratio));
        }

        public override void Update(GameTime gameTime)
        {
            if (!initialized)
            {
                Reset();
            }

            Source.X += Speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Source.Y += Speed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //    Finished = true;
        }

    }
}