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

        public Scriptable()
        {
            Actions = new ActionQueue();
        }
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
                var s = new Scriptable() {Name = name, Object = obj};
                Scriptables.Add(name, s);
            }
        }

        private List<Scriptable> deferred = new List<Scriptable>(); 
        public void DeferredAdd(string name, IScriptable obj, List<IAction> actions )
        {
            var s = new Scriptable() {Name = name, Object = obj};
            foreach (var action in actions)
            {
                action.Engine = this;
                s.Actions.Add(action);
            }

            deferred.Add(s); 
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

        public void Update(GameTime gameTime)
        {
            foreach (var s in Scriptables.Values.Where(s => s.Object.Enabled))
            {
                s.Actions.Update(gameTime);
            }

            foreach (var d in deferred)
            {
                Scriptables.Add(d.Name, d);
            }
            deferred.Clear();
        }
    }
}