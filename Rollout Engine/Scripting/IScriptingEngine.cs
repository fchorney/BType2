using Microsoft.Xna.Framework;
using Rollout.Scripting;

namespace Rollout.Scripting
{
    public interface IScriptingEngine
    {
        void Add(string name, IScriptable scriptable);
        void AddScript(IScript script);
        void Update(GameTime gameTime);
        IScriptable this[string name] { get; }
    }
}