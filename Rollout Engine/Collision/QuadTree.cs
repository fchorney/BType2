using Microsoft.Xna.Framework;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rollout.Primitives;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace Rollout.Collision
{
    public class QuadTree : Rectangle
    {
        private QuadTree root;
        public QuadTree[] Children { get; private set; }
        public int Threshold = 4;
        public bool Enabled;
        public int PrimaryCount;

        private VectorList<ICollidable> Objects;
        private PrimitiveLine sprite;

        public Vector2D Offset;

        public QuadTree(double x, double y, double w, double h)
            : base(x, y, w, h)
        {
            Objects = new VectorList<ICollidable>();
            Offset = new Vector2D();
            root = this;
            Enabled = false;

            sprite = new PrimitiveLine() { Colour = Color.Red };
            sprite.CreateRectangle(this);

        }

        private QuadTree(double x, double y, double w, double h, QuadTree root) : this(x, y, w, h)
        {
            this.root = root;
        }

        public void Split()
        {
            if(Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Split();
                } 
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

        public void Reset()
        {
            Enabled = false;
            Objects.Clear();
            PrimaryCount = 0;

            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Reset();
                }
            }
        }

        public void Add(ICollidable obj)
        {
            if(!IsDivided())
            {
                Objects.Add(obj);

                if (obj.Primary)
                    PrimaryCount++;

                if (PrimaryCount > 0 && Objects.Count > Threshold && Divide())
                {
                    Add(Objects);
                    Objects.Clear();
                }

            }
            else
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    if (Children[i].Enabled && Children[i].QuadIntersects(obj.Shape))
                    {
                        Children[i].Add(obj);
                    }
                }
            }

        }


        public void Add(VectorList<ICollidable> obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                Add(obj[i]);
            }
        }

        protected bool IsDivided()
        {
            return Children != null && Children[0].Enabled;
        }


        public PairList<ICollidable> GetCollisions()
        {

            var Collisions = new PairList<ICollidable>();

            CheckCollisions(Collisions);

            return Collisions;
        }


        private bool Divide()
        {
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Enabled = true;
                }
                return true;
            }
            return false;
        }

        private bool QuadIntersects(IShape obj)
        {
            if (obj == null) return false;

            return   !(root.Offset.X + X > obj.X + obj.W || root.Offset.X + X + W < obj.X ||
                       root.Offset.Y + Y > obj.Y + obj.H || root.Offset.Y + Y + H < obj.Y);
        }

        private void CheckCollisions(PairList<ICollidable> collisions)
        {
            if (!IsDivided() && Objects.Count > 1 && PrimaryCount > 0)
            {
                for (var i = 0; i < Objects.Count - 1; i++)
                {
                    for (var j = i + 1; j < Objects.Count; j++)
                    {
                        if (!Objects[i].Primary && !Objects[j].Primary) continue;
                        if (!Objects[i].Enabled || !Objects[j].Enabled) continue;
                        if (!Objects[i].Shape.Intersects(Objects[j].Shape)) continue;

                        collisions.Add(Objects[i], Objects[j]);
                    }
                }
            }
            else 
            {
                if (Children != null)
                    for (int i = 0; i < Children.Length; i++)
                    {
                        Children[i].CheckCollisions(collisions);
                    }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!IsDivided())
            {
                sprite.Draw(gameTime);
                G.SpriteBatch.DrawString(PrimaryCount.ToString(), new Vector2((float)X,(float)Y));
            }
            else
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Draw(gameTime);
                }
            }
        } 
    }
}
