namespace Rollout.Collision
{
    public interface ICollisionEngine
    {
        void Add(ICollidable obj);
        void ProcessCollisions();
    }
}