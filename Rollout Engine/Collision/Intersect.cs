using System;
using Rollout.Collision.Shapes;

namespace Rollout.Collision
{
    public class Intersect
    {
        public static bool EverywhereToShape(Everywhere a, IShape b)
        {
            return true;
        }

        public static bool CircleToCircle(Circle a, Circle b)
        {

            double c2 = (a.cX - b.cX) * (a.cX - b.cX) + (a.cY - b.cY) * (a.cY - b.cY);
            double r2 = (a.R + b.R) * (a.R + b.R);

            return c2 <= r2;
        }

        public static bool CircleToRectangle(Circle a, Rectangle b)
        {
            //double cdX = Math.Abs(a.X - b.X - b.W/2);
            //double cdY = Math.Abs(a.Y - b.Y - b.H/2);

            //if (cdX > (b.W/2 + a.R))
            //{
            //    return false;
            //}
            //if (cdY > (b.H/2 + a.R))
            //{
            //    return false;
            //}

            //if (cdX <= (b.W/2))
            //{
            //    return true;
            //}
            //if (cdY > (b.H/2))
            //{
            //    return true;
            //}

            //double cornerDistance_sq = ((cdX - b.W/2)*(cdX - b.W/2)) +
            //                           ((cdY - b.H/2)*(cdY - b.H/2));
           
            //return (cornerDistance_sq <= (a.R*a.R));

            double dmin = 0;
            double[] C = new[] { a.cX, a.cY };
            double[] Bmin = new[] { b.X, b.Y };
            double[] Bmax = new[] { b.X + b.W, b.Y + b.H };
            for (int i = 0; i < C.Length; i++)
            {
                if (C[i] < Bmin[i]) dmin += (C[i] - Bmin[i]) * (C[i] - Bmin[i]);
                else if (C[i] > Bmax[i]) dmin += (C[i] - Bmax[i]) * (C[i] - Bmax[i]);
            }
            return dmin <= a.R * a.R;
        }

        public static bool RectangleToRectangle(Rectangle a, Rectangle b)
        {
            if (b == null) return false;
            return !(a.X > b.X + b.W || a.X + a.W < b.X ||
                     a.Y > b.Y + b.H || a.Y + a.H < b.Y);
        }


    }
}
