using System;
using Microsoft.Xna.Framework;

namespace Rollout.Collision
{

    public interface ICollisionEngine
    {
        void Add(ICollidable obj);
        void Update(GameTime gameTime);
        void Register<TSender, TObject>(Action<TSender,TObject> eventHandler) where TSender : ICollidable where TObject : ICollidable; 
        void Draw(GameTime gameTime);
    }
}