using Rollout.Collision.Shapes;

namespace Rollout.Collision
{

    public interface ICollidable
    {
        IShape Shape { get; }
        bool Primary { get; set; }
        bool Enabled { get; set; }
    }
}