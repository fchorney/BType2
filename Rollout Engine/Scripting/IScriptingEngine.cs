using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public interface IScriptingEngine
    {
        void Add(string name, IScriptable scriptable, List<IAction> actions = null);
        void AttachAction(string name, IAction action);
        void ResetTarget(string name);
        void ClearTarget(string name);
        void Update(GameTime gameTime);
        void Load(string assetName);
        IScriptable this[string name] { get; }
        IScriptProvider Provider { get; }

        Dictionary<string, Type> SpriteTypes { get; set; }
        Dictionary<string, XElement> Templates { get; set; }
    }
}