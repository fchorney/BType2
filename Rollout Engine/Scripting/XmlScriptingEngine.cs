using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public class XmlScriptingEngine: IScriptingEngine
    {
        private bool IsUpdating { get; set; }

        private List<Scriptable> AddQueue { get; set; } 
        private Dictionary<string, Scriptable> Scriptables { get; set; }

        private IScriptProvider provider;
        public IScriptProvider Provider
        {
            get { return provider; }
        }

        public Dictionary<string, Type> SpriteTypes { get; set; }
        public Dictionary<string, XElement> Templates { get; set; }

        public XmlScriptingEngine()
        {
            provider = new XmlScriptProvider();
            Scriptables = new Dictionary<string, Scriptable>();
            AddQueue = new List<Scriptable>();
            IsUpdating = false;

            SpriteTypes = new Dictionary<string, Type>();
            Templates = new Dictionary<string, XElement>();
        }

        public void Add(string name, IScriptable obj, List<IAction> actions = null)
        {
            if(!Scriptables.ContainsKey(name))
            {
                var s = new Scriptable() {Name = name, Object = obj};

                if (actions != null)
                    foreach (var action in actions)
                    {
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

        public void AttachAction(string name, IAction action)
        {
            if (Scriptables.ContainsKey(name))
            {
                Scriptables[name].Actions.Add(action);
            }
            else
            {
                foreach (var scriptable in AddQueue)
                {
                    if (scriptable.Name != name) continue;
                    scriptable.Actions.Add(action);
                    break;
                }
            }
        }

        public IScriptable this[string name]
        {
            get { return Scriptables[name].Object; }
        }

        public void ClearTarget(string name)
        {
            if (name != null && Scriptables.ContainsKey(name))
                Scriptables[name].Actions.Clear();
        }

        public void ResetTarget(string name)
        {
            if (name != null && Scriptables.ContainsKey(name))
                Scriptables[name].Actions.Reset();
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

        public void Load(string assetName)
        {
            Provider.Load(assetName);
        }
    }
}