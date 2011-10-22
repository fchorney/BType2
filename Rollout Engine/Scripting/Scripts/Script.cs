using System;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Scripts
{
    public abstract class Script : IScript
    {
        public DateTime StartTime { get; set; }

        public Script(DateTime startTime)
        {
            StartTime = startTime;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}