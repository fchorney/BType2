using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rollout.Collision
{
    public interface ICollidable
    {
        IShape Shape { get; }
        void Collide(ICollidable Obj);
    }

    public class QuadTree : Rectangle
    {
        QuadTree Root;
        QuadTree[] Children;

        List<ICollidable> Objects;
        PairList<ICollidable> Collisions;

        public Vector2D Offset;

        public QuadTree(double x, double y, double w, double h)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            Root = this;
        }

        private QuadTree(double x, double y, double w, double h, QuadTree Root)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            this.Root = Root;
        }

        private bool QuadIntersects(IShape obj)
        {
            if (obj == null) return false;
            return MathUtil.CheckAxis(Root.Offset.X + X, Root.Offset.X + X + W, obj.X, obj.X + obj.W) &&
                   MathUtil.CheckAxis(Root.Offset.Y + Y, Root.Offset.Y + Y + H, obj.Y, obj.Y + obj.H);
        }

        public void Split()
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Split();
            else
            {
                double hW = W / 2;
                double hY = H / 2;

                Children = new QuadTree[4];
                Children[0] = new QuadTree(X, Y, hW, hY, Root);
                Children[1] = new QuadTree(X + hW, Y, hW, hY, Root);
                Children[2] = new QuadTree(X, Y + hY, hW, hY, Root);
                Children[3] = new QuadTree(X + hW, Y + hY, hW, hY, Root);

            }
        }

        public void Add(ICollidable Obj)
        {

            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].QuadIntersects(Obj.Shape))
                        Children[i].AddToChildren(Obj);
            }
            else
            {
                Objects.Add(Obj);
            }

        }

        private void AddToChildren(ICollidable Obj)
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].QuadIntersects(Obj.Shape))
                        Children[i].AddToChildren(Obj);
            }
            else
            {
                Objects.Add(Obj);
            }
        }

        public void Clear()
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    Children[i].Clear();
            }
            else
            {
                Objects.Clear();
            }
        }

        public PairList<ICollidable> GetCollisions()
        {
            return Collisions;
        }

        public void CheckCollisions()
        {
            Collisions.Clear();
            CheckChildCollisions(Collisions);
        }

        private void CheckChildCollisions(PairList<ICollidable> CollisionList)
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].CheckChildCollisions(CollisionList);
            else
            {
                Collisions.Clear();
                if (Objects.Count >= 2)
                    for (int i = 0; i < Objects.Count - 1; i++)
                        for (int j = i + 1; j < Objects.Count; j++)
                            if (Objects[i].Shape.Intersects(Objects[j].Shape))
                            {
                                CollisionList.Add(Objects[i], Objects[j]);
                                Collisions.Add(Objects[i], Objects[j]);
                            }
            }
        }
    }
}
