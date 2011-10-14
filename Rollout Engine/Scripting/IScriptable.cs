using System.Collections.Generic;

namespace Rollout.Scripting
{
    public interface IScriptable
    {
        string Name { get; }
        List<IAction> Actions { get; }
        bool Enabled { get; set; }
    }
}