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

    public class XmlScriptingEngine: IScriptingEngine
    {
        private bool IsUpdating { get; set; }

        private List<Scriptable> AddQueue { get; set; } 
        private Dictionary<string, Scriptable> Scriptables { get; set; }

        public XmlScriptingEngine()
        {
            Scriptables = new Dictionary<string, Scriptable>();
            AddQueue = new List<Scriptable>();
            IsUpdating = false;
        }

        public void Add(string name, IScriptable obj, List<IAction> actions = null)
        {
            if(!Scriptables.ContainsKey(name))
            {
                var s = new Scriptable() {Name = name, Object = obj};

                if (actions != null)
                    foreach (var action in actions)
                    {
                        action.Engine = this;
                        s.Actions.Add(action);
                    }

                //dont allow the Scriptable dictionary to be modified during iteration
                //add the object to a queue so we can defer adding until Update() is complete
                if (IsUpdating)
                    AddQueue.Add(s); 
                else
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

        public void Update(GameTime gameTime)
        {
            IsUpdating = true;

            foreach (var s in Scriptables.Values.Where(s => s.Object.Enabled))
            {
                s.Actions.Update(gameTime);
            }

            //if we generated any Scriptable objects during the update, add them after we're done iterating
            foreach (var s in AddQueue)
            {
                Scriptables.Add(s.Name, s);
            }
            AddQueue.Clear();

            IsUpdating = false;
        }
    }
}