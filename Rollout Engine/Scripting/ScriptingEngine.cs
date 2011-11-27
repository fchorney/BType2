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
        public ActionQueue Actions { get; set; } 
    }

    public class ScriptingEngine: IScriptingEngine
    {
        private List<IScript> Scripts { get; set; }
        private Dictionary<string, Scriptable> Scriptables { get; set; }

        public ScriptingEngine()
        {
            Scripts = new List<IScript>();
            Scriptables = new Dictionary<string, Scriptable>();
        }

        public void Add(string name, IScriptable obj)
        {
            //name = name.ToLower();
            if(!Scriptables.ContainsKey(name))
            {
                var s = new Scriptable() {Name = name, Object = obj, Actions = new ActionQueue()};
                Scriptables.Add(name, s);
            }
        }

        public void AddAction(string name, IAction action)
        {
            action.Engine = this;
            Scriptables[name].Actions.Add(action);
        }

        public IScriptable this[string name]
        {
            get { return Scriptables[name].Object; }
        }

        public void AddScript(IScript script)
        {
            Scripts.Add(script);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var s in Scriptables.Values.Where(s => s.Object.Enabled))
            {
                s.Actions.Update(gameTime);
            }

            foreach (var script in Scripts)
            {
                script.Update(gameTime);
            }

        }
    }
}