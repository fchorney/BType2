using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Scripting;

namespace Rollout.Scripting
{
    public interface IScriptingEngine
    {
        void Add(string name, IScriptable scriptable, List<IAction> actions = null);
        void AddAction(string name, IAction action);
        void Update(GameTime gameTime);
        IScriptable this[string name] { get; }
    }
}