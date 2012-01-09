namespace Rollout.Collision
{
    public delegate void CollisionHandler(ICollidable source, ICollidable obj);

    public interface ICollidable
    {
        IShape Shape { get; }
        CollisionHandler OnCollision { get; }
        bool Enabled { get; set; }
    }
}