using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public interface IAction
    {

        void Update(GameTime gameTime);
        void Reset();

        bool Finished { get; set; }

        List<IAction> Actions { get; }
        List<IAction> ActionQueue { get; }

        IScriptingEngine Engine { get; }

    }
}