using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rollout.Primitives;

namespace Rollout.Collision.Shapes
{
    public class QuadTree : Rectangle
    {
        private QuadTree root;
        public QuadTree[] Children { get; private set; }

        private List<ICollidable> Objects;
        private PairList<ICollidable> Collisions;

        public List<PrimitiveLine> ShapeSprites = new List<PrimitiveLine>(); 

        public Vector2D Offset;

        public QuadTree(double x, double y, double w, double h)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            root = this;
        }

        private QuadTree(double x, double y, double w, double h, QuadTree root)
            : base(x, y, w, h)
        {
            Objects = new List<ICollidable>();
            Collisions = new PairList<ICollidable>();
            Offset = new Vector2D();
            this.root = root;
        }

        public void Add(ICollidable obj)
        {            
            if (CollisionEngine.Debug && obj.Shape != null)
            {
                var pl = new PrimitiveLine(obj);
                ShapeSprites.Add(pl);
            }

            if (Children != null)
                foreach (var t in Children.Where(t => t.QuadIntersects(obj.Shape)))
                    t.AddToChildren(obj);
            else
                Objects.Add(obj);
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
            {
                foreach (var t in Children)
                    t.Split();
            }
            else
            {
                var hW = W / 2;
                var hY = H / 2;

                Children = new QuadTree[4];
                Children[0] = new QuadTree(X, Y, hW, hY, root);
                Children[1] = new QuadTree(X + hW, Y, hW, hY, root);
                Children[2] = new QuadTree(X, Y + hY, hW, hY, root);
                Children[3] = new QuadTree(X + hW, Y + hY, hW, hY, root);

            }
        }

        protected void Clear()
        {
            if (Children != null)
                foreach (var t in Children)
                    t.Clear();
            else
                Objects.Clear();
        }

        private bool QuadIntersects(IShape obj)
        {
            if (obj == null) return false;
            return MathUtil.CheckAxis(root.Offset.X + X, root.Offset.X + X + W, obj.X, obj.X + obj.W) &&
                   MathUtil.CheckAxis(root.Offset.Y + Y, root.Offset.Y + Y + H, obj.Y, obj.Y + obj.H);
        }

        private void AddToChildren(ICollidable obj)
        {
            if (Children != null)
                foreach (var t in Children.Where(t => t.QuadIntersects(obj.Shape)))
                    t.AddToChildren(obj);
            else
                Objects.Add(obj);
        }

        private void CheckChildCollisions(PairList<ICollidable> collisionList)
        {
            if (Children != null)
            {
                foreach (var t in Children)
                    t.CheckChildCollisions(collisionList);
            }
            else
            {
                Collisions.Clear();
                if (Objects.Count >= 2)
                {
                    for (var i = 0; i < Objects.Count - 1; i++)
                    {
                        for (var j = i + 1; j < Objects.Count; j++)
                        {
                            if (!Objects[i].Enabled || !Objects[j].Enabled) continue;
                            if (!Objects[i].Shape.Intersects(Objects[j].Shape)) continue;

                            collisionList.Add(Objects[i], Objects[j]);
                            Collisions.Add(Objects[i], Objects[j]);
                        }
                    }
                }
            }
        }
    }
}
