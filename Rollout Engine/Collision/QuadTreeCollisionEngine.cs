using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Primitives;

namespace Rollout.Collision.Shapes
{
    public class QuadTreeCollisionEngine : ICollisionEngine
    {
        private QuadTree quadTree;
        private List<PrimitiveLine> quadSprites;

        private const int Split = 4;

        public QuadTreeCollisionEngine()
        {
            quadTree = new QuadTree(0, 0, G.Game.GraphicsDevice.Viewport.Width, G.Game.GraphicsDevice.Viewport.Height);

            for (var i = 0; i < Split; i++)
            {
                quadTree.Split();
            }

            if (CollisionEngine.Debug)
            {
                quadSprites = new List<PrimitiveLine>();
                CreateQuadSprites(quadTree);
            }
        }

        public void Add(ICollidable obj)
        {
            quadTree.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            PairList<ICollidable> collisions = quadTree.GetCollisions();

            for (var i = 0; i < collisions.Count; i++)
            {
                Set<ICollidable> collision = collisions.Get(i);
                ICollidable a = collision.Get(0);
                ICollidable b = collision.Get(1);

                a.OnCollision(a, b);
                b.OnCollision(b, a);
            }

            if (CollisionEngine.Debug)
            {
                foreach (var primitive in quadTree.ShapeSprites)
                {
                    primitive.Update(gameTime);
                }
            }
        }
        public void Draw(GameTime gameTime)
        {
            foreach (var primitive in quadSprites)
            {
                primitive.Draw(gameTime);
            }
            foreach (var primitive in quadTree.ShapeSprites)
            {
                primitive.Draw(gameTime);
            }
        }

        private void CreateQuadSprites(QuadTree tree)
        {
            var p = new PrimitiveLine() { Colour = Color.Red };
            p.CreateRectangle(tree);
            quadSprites.Add(p);

            if (tree.Children == null) return;
            foreach (var t in tree.Children)
                CreateQuadSprites(t);
        } 
    }
}
