using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public interface IAction
    {
        void Update(GameTime gameTime);
        void Reset();

        bool Finished { get; set; }

        void AddAction(IAction action);
        
        ActionQueue Actions { get; }

        bool Wait { get; set; }
    }
}