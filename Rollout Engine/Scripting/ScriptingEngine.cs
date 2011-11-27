using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    class Scriptable
    {
        public string Name { get; set; }
        public IScriptable Object { get; set; }
        public List<IAction> Actions { get; set; } 
    }

    public class ScriptingEngine: IScriptingEngine
    {
        private Dictionary<string, Scriptable> Scriptables { get; set; }

        public ScriptingEngine()
        {
            Scriptables = new Dictionary<string, Scriptable>();
        }

        public void Add(string name, IScriptable obj)
        {
            //name = name.ToLower();
            if(!Scriptables.ContainsKey(name))
            {
                var s = new Scriptable() {Name = name, Object = obj, Actions = new List<IAction>()};
                Scriptables.Add(name, s);
            }
        }

        public void AddAction(string name, IAction action)
        {
            action.Engine = this;

            foreach(IAction child in action.Actions)
            {
                child.Engine = this;
            }

            Scriptables[name].Actions.Add(action);
        }

        public IScriptable this[string name]
        {
            get { return Scriptables[name].Object; }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var s in Scriptables.Values.Where(s => s.Object.Enabled))
            {
                foreach (var action in s.Actions.Where(action => !action.Finished))
                {
                    action.Update(gameTime);
                }
            }
        }
    }
}