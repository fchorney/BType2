using Microsoft.Xna.Framework;

namespace Rollout.Collision.Shapes
{
    public interface ICollisionEngine
    {
        void Add(ICollidable obj);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}