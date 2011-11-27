using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public interface IAction
    {
        void Update(GameTime gameTime);
        void Reset();

        bool Finished { get; set; }

        void AddAction(IAction action, bool autoWait = false);
        
        ActionQueue Actions { get; }

        IScriptingEngine Engine { get; set; }

        bool Wait { get; set; }
    }
}