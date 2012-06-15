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
        private Dictionary<int, CollisionHandler> CollisionHandlers { get; set; }
        private List<ICollidable> objects;

        public List<PrimitiveLine> ShapeSprites = new List<PrimitiveLine>(); 

        public QuadTreeCollisionEngine()
        {
            quadTree = new QuadTree(0, 0, G.Game.GraphicsDevice.Viewport.Width, G.Game.GraphicsDevice.Viewport.Height);

            for (int i = 0; i < 3; i++)
            {
                quadTree.Split();
            }

            objects = new List<ICollidable>();

            CollisionHandlers = new Dictionary<int, CollisionHandler>();
        }


        public void Add(ICollidable obj)
        {
            objects.Add(obj);

            if (CollisionEngine.Debug && obj.Shape != null)
            {
                var pl = new PrimitiveLine(obj);
                ShapeSprites.Add(pl);
            }
        }

        public void Update(GameTime gameTime)
        {
            quadTree.Reset();

            for (int i = 0; i < objects.Count; i++)
            {
                quadTree.Add(objects[i]);
            }

            PairList<ICollidable> collisions = quadTree.GetCollisions();

            for (var i = 0; i < collisions.Count; i++)
            {
                Set<ICollidable> collision = collisions.Get(i);
                ICollidable a = collision.Get(0);
                ICollidable b = collision.Get(1);

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
                foreach (var primitive in ShapeSprites)
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
            quadTree.Draw(gameTime);

            foreach (var primitive in ShapeSprites)
            {
                primitive.Draw(gameTime);
            }
        }
    }
}
