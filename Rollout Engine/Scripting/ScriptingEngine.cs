using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public class ScriptingEngine: IScriptingEngine
    {
        private List<IScript> Scripts { get; set; }
        protected Dictionary<string, IScriptable> Scriptables { get; set; }

        private List<IAction> Actions { get; set; }

        private static IScriptingEngine instance;
        public static IScriptingEngine Instance
        {
            get { return instance ?? (instance = new ScriptingEngine()); }
        }

        private ScriptingEngine()
        {
            Scripts = new List<IScript>();
            Scriptables = new Dictionary<string, IScriptable>();

            Actions = new List<IAction>();
        }

        public void Add(IScriptable obj)
        {
            if(!Scriptables.ContainsKey(obj.Name)) //TODO: get rid of this
            Scriptables.Add(obj.Name, obj);
        }

        public IScriptable this[string name]
        {
            get { return Scriptables[name]; }
        }

        public void AddScript(IScript script)
        {
            Scripts.Add(script);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var s in Scriptables.Values.Where(s => s.Enabled))
            {
                foreach (var action in s.Actions.Where(action => !action.Finished))
                {
                    action.Update(gameTime);
                }
            }

            foreach (var script in Scripts)
            {
                script.Update(gameTime);
            }

        }


    }
}