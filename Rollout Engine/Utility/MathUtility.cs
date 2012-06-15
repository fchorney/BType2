using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Collision.Shapes;
using Rollout.Drawing;
using Rollout.Drawing.Sprites;

namespace Rollout.Utility
{
    class MathUtility
    {
        public static Vector2 GetCenter(Sprite obj)
        {
            return new Vector2(obj.X + (float)obj.W/2, obj.Y + (float)obj.H/2);
        }

        public static Vector2 GetCenter(IShape obj)
        {
            return new Vector2((float)(obj.X + obj.W/2), (float)(obj.Y + obj.H/2));
        }
    }
}
