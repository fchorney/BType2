using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Screens;

namespace Rollout.Collision
{
    public static class CollisionEngine
    {
        private static Dictionary<string, ICollisionEngine> Engines = new Dictionary<string, ICollisionEngine>();
        public static ICollisionEngine Engine
        {
            get
            {
                if (!Engines.ContainsKey(ScreenContext.ContextKey))
                    Engines.Add(ScreenContext.ContextKey, new QuadTreeCollisionEngine());
                return Engines[ScreenContext.ContextKey];
            }
        }

        public static void Add(ICollidable obj)
        {
            Engine.Add(obj);
        }

        public static void Update(GameTime gameTime)
        {
            Engine.Update(gameTime);
        }
    }
}