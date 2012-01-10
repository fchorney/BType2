using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public interface IScriptingEngine
    {
        void Add(string name, IScriptable scriptable, List<IAction> actions = null);
        void AddAction(string name, IAction action);
        void ResetActionQueue(string name);
        void Update(GameTime gameTime);
        IScriptable this[string name] { get; }
    }
}