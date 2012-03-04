using Rollout.Drawing;

namespace Rollout.Scripting
{
    public interface IScriptable : ITransformable
    {
        bool Enabled { get; set; }
    }
}