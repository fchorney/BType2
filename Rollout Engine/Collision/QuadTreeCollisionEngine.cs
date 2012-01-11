using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Primitives;

namespace Rollout.Collision
{
    public class QuadTreeCollisionEngine : ICollisionEngine
    {
        private QuadTree quadTree;
        private List<PrimitiveLine> quadSprites;
        private Dictionary<int, CollisionHandler> CollisionHandlers { get; set; }

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

            CollisionHandlers = new Dictionary<int, CollisionHandler>();

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

                //if (a.OnCollision != null) a.OnCollision(a, b);
                //if (b.OnCollision != null) b.OnCollision(b, a);

                var key = GetTypeHashKey(a.GetType(), b.GetType());
                if (CollisionHandlers.ContainsKey(key))
                {
                    var handler = CollisionHandlers[key];

                    if (a.GetType() == handler.Sender)
                        handler.Handler.Invoke(a, b);
                    else
                        handler.Handler.Invoke(b, a);

                }
                
            }

            if (CollisionEngine.Debug)
            {
                foreach (var primitive in quadTree.ShapeSprites)
                {
                    primitive.Update(gameTime);
                }
            }
        }

        class CollisionHandler
        {
            public Type Sender { get; set; }
            public Type Object { get; set; }
            public Action<ICollidable, ICollidable> Handler { get; set; }

            public CollisionHandler(Type s, Type o, Action<ICollidable, ICollidable> handler)
            {
                Sender = s;
                Object = o;
                Handler = handler;
            }

            public int HashKey
            {
                get { return GetTypeHashKey(Sender, Object); }
            }
        }

        private static int GetTypeHashKey(Type t, Type u)
        {
            return t.GetHashCode() ^ u.GetHashCode();
        } 


        public void Register<TSender, TObject>(Action<TSender, TObject> eventHandler)
            where TSender : ICollidable 
            where TObject : ICollidable
        {

            var handler = new CollisionHandler(typeof (TSender), typeof (TObject), (x, y) => eventHandler.Invoke((TSender)x, (TObject)y));

            CollisionHandlers.Add(handler.HashKey, handler);
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
