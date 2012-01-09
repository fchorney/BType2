using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rollout.Primitives;

namespace Rollout.Collision
{
    class QuadTree : Rectangle
    {
        QuadTree Root;
        QuadTree[] Children;

        List<ICollidable> Objects;
        PairList<ICollidable> Collisions;

#if DEBUG
        public List<PrimitiveLine> shapeSprites = new List<PrimitiveLine>(); 
#endif

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

        public void Add(ICollidable obj)
        {
#if DEBUG
            if (obj.Shape != null)
            {
                var pl = new PrimitiveLine(obj);
                shapeSprites.Add(pl);
            }
#endif
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].QuadIntersects(obj.Shape))
                        Children[i].AddToChildren(obj);
            }
            else
            {
                Objects.Add(obj);
            }

        }

        public PairList<ICollidable> GetCollisions()
        {
            Collisions.Clear();
            CheckChildCollisions(Collisions);
            return Collisions;
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

        protected void Clear()
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

        private bool QuadIntersects(IShape obj)
        {
            if (obj == null) return false;
            return MathUtil.CheckAxis(Root.Offset.X + X, Root.Offset.X + X + W, obj.X, obj.X + obj.W) &&
                   MathUtil.CheckAxis(Root.Offset.Y + Y, Root.Offset.Y + Y + H, obj.Y, obj.Y + obj.H);
        }

        private void AddToChildren(ICollidable obj)
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                    if (Children[i].QuadIntersects(obj.Shape))
                        Children[i].AddToChildren(obj);
            }
            else
            {
                Objects.Add(obj);
            }
        }

        private void CheckChildCollisions(PairList<ICollidable> collisionList)
        {
            if (Children != null)
                for (int i = 0; i < Children.Length; i++)
                    Children[i].CheckChildCollisions(collisionList);
            else
            {
                Collisions.Clear();
                if (Objects.Count >= 2)
                    for (int i = 0; i < Objects.Count - 1; i++)
                        for (int j = i + 1; j < Objects.Count; j++)
                            if (Objects[i].Enabled && Objects[j].Enabled && Objects[i].Shape.Intersects(Objects[j].Shape))
                            {
                                collisionList.Add(Objects[i], Objects[j]);
                                Collisions.Add(Objects[i], Objects[j]);
                            }
            }
        }
    }
}
