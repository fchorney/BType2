using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Primitives;

namespace Rollout.Collision
{
    public class QuadTreeCollisionEngine : ICollisionEngine
    {
        private QuadTree quadTree;
        private List<PrimitiveLine> quadSprites;

        public QuadTreeCollisionEngine()
        {
            quadTree = new QuadTree(0, 0, G.Game.GraphicsDevice.Viewport.Width, G.Game.GraphicsDevice.Viewport.Height);

            quadTree.Split();
            quadTree.Split();
            quadTree.Split();
            //quadTree.Split();

            if (CollisionEngine.Debug)
            {
                quadSprites = new List<PrimitiveLine>();
                CreateQuadSprites(quadTree);
            }

            CollisionHandlers = new Dictionary<int, Action<ICollidable, ICollidable>>();

        }

        public void Add(ICollidable obj)
        {
            quadTree.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            PairList<ICollidable> collisions = quadTree.GetCollisions();

            for (int i = 0; i < collisions.Count; i++)
            {
                Set<ICollidable> collision = collisions.Get(i);
                ICollidable a = collision.Get(0);
                ICollidable b = collision.Get(1);

                
                if (a.OnCollision != null) a.OnCollision(a, b);
                if (b.OnCollision != null) b.OnCollision(b, a);

                int key = GetTypeHashKey(a.GetType(), b.GetType());
                if (CollisionHandlers.ContainsKey(key))
                {
                    CollisionHandlers[key].Invoke(a,b);
                }
                
            }

            if (CollisionEngine.Debug)
            {
                foreach (var primitive in quadTree.shapeSprites)
                {
                    primitive.Update(gameTime);
                }
            }
        }

        private Dictionary<int, Action<ICollidable, ICollidable>> CollisionHandlers { get; set; }

        public void Register<TSender, TObject>(Action<TSender, TObject> eventHandler)
            where TSender : ICollidable 
            where TObject : ICollidable
        {
            Action<ICollidable, ICollidable> action = eventHandler as Action<ICollidable, ICollidable>;

            CollisionHandlers.Add(GetTypeHashKey(typeof(TSender), typeof(TObject)), action);
        }

        private int GetTypeHashKey(Type T, Type U)
        {
            return T.GetHashCode() ^ U.GetHashCode();
        } 

        public void Draw(GameTime gameTime)
        {
            foreach (var primitive in quadSprites)
            {
                primitive.Draw(gameTime);
            }
            foreach (var primitive in quadTree.shapeSprites)
            {
                primitive.Draw(gameTime);
            }
        }

        private void CreateQuadSprites(QuadTree tree)
        {
            var p = new PrimitiveLine() { Colour = Color.Red };
            p.CreateRectangle(new Rectangle(tree.X, tree.Y, tree.W, tree.H));
            quadSprites.Add(p);

            if (tree.Children == null) return;
            foreach (var t in tree.Children)
            {
                CreateQuadSprites(t);
            }
        } 
    }
}
