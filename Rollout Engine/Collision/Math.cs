using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rollout.Collision
{
    public class MathUtil
    {
        public static bool CheckAxis(double x1, double x2, double y1, double y2)
        {
            if (x1 > x2)
            {
                double temp = x1;
                x2 = x1;
                x2 = temp;
            }

            if (y1 > y2)
            {
                double temp = y1;
                y2 = y1;
                y2 = temp;
            }

            return (y1 >= x1 && y1 <= x2)
                || (y2 >= x1 && y2 <= x2)
                || (x1 >= y1 && x1 <= y2)
                || (x2 >= y1 && x2 <= y2);
        }

        public static double AngleTo(Point from, Point to)
        {
            return from.AngleTo(to);
        }

    }

    public class Point
    {
        protected double x;
        protected double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Point() : this(0, 0) { }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(double x, double y)
        {
            this.x = (float)x;
            this.y = (float)y;
        }

        public double AngleTo(Point p)
        {
            return (float)Math.Atan((p.y - y) / (p.x - x));
        }

        public double DistanceTo(Point p)
        {
            return (float)Math.Sqrt((p.y - y) * (p.y - y) + (p.x - x) * (p.x - x));
        }

        public double DotProduct(Point p)
        {
            return DotProduct(p.x, p.y);
        }

        public double DotProduct(double x, double y)
        {
            if (x == 0 && y == 0) return 0;
            return (this.x * x + this.y * y) / (x * x + y * y);
        }

        public virtual Point Projection(Point p)
        {
            Point proj = new Point();

            double dp = DotProduct(p);
            proj.x = dp * p.x;
            proj.y = dp * p.y;

            return proj;
        }

        public static Point operator +(Point v1, Point v2)
        {
            return new Point(v1.x + v2.x, v1.y + v2.y);
        }

        public static Point operator -(Point v1, Point v2)
        {
            return new Point(v2.x - v1.x, v2.y - v1.y);
        }

        public override string ToString()
        {
            return "[" + (int)x + ", " + (int)y + "]";
        }
    }

    public class Vector2D : Point
    {
        protected Point position;

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2D() : this(0, 0) { }

        public Vector2D(double x, double y)
            : base(x, y)
        {
            position = new Point();
        }

        public Vector2D(double x, double y, double px, double py)
            : base(x, y)
        {
            position = new Point(px, py);
        }

        public Vector2D(double x, double y, Point p)
            : base(x, y)
        {
            position = p;
        }

        public void Set(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public virtual Vector2D Projection(Vector2D v)
        {
            Vector2D proj = new Vector2D();
            Point p = v.Position - position;


            double dp = DotProduct(v);
            proj.x = dp * v.x;
            proj.y = dp * v.y;

            p = p.Projection(v);

            proj.position.X = v.position.X + p.X;
            proj.position.Y = v.position.Y + p.Y;

            return proj;
        }

        public Vector2D XComponent
        {
            get { return new Vector2D(x, 0, position); }
        }
        public Vector2D YComponent
        {
            get { return new Vector2D(0, y, position); }
        }

        public void Clear()
        {
            position.X = 0;
            position.Y = 0;
            x = 0;
            y = 0;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v2.x - v1.x, v2.y - v1.y);
        }
    }

    public class PairList<T>
    {
        List<Set<T>> list = new List<Set<T>>();

        public int Count
        {
            get { return list.Count; }
        }

        public void Add(T a, T b)
        {
            Set<T> p = new Set<T>(new T[] { a, b });

            if (!list.Contains(p))
                list.Add(p);
        }

        public Set<T> Get(int i)
        {
            return list[i];
        }

        public void Clear()
        {
            list.Clear();
        }

    }

    public class Set<T>
    {
        private List<T> objects;
        private int hashCode;

        public Set()
        {
            objects = new List<T>();
            hashCode = 0;
        }

        public Set(T[] list)
            : this()
        {
            for (int i = 0; i < list.Length; i++)
                Add(list[i]);
        }

        public void Add(T obj)
        {
            objects.Add(obj);
            hashCode ^= obj.GetHashCode();
        }

        public T Get(int i)
        {
            return objects[i];
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override string ToString()
        {
            string s = "";

            foreach (T obj in objects)
                s += obj.ToString() + ", ";
            s = s.Substring(0, s.Length - 2);

            return "{" + s + "}";
        }
    }
}
