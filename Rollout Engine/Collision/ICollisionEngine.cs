using Microsoft.Xna.Framework;

namespace Rollout.Collision
{
    public interface ICollisionEngine
    {
        void Add(ICollidable obj);
        void Update(GameTime gameTime);
    }
}