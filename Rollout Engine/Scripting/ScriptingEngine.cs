using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    public class ScriptingEngine: IScriptingEngine
    {
        private List<IScript> Scripts { get; set; }
        private List<IScriptable> Scriptables { get; set; }

        public ScriptingEngine()
        {
            Scripts = new List<IScript>();
            Scriptables = new List<IScriptable>();
        }

        public void Add(IScriptable obj)     
        {
            Scriptables.Add(obj);
        }

        public void AddScript(IScript script)
        {
            Scripts.Add(script);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var script in Scripts)
            {
                script.Update(gameTime);
            }

        }


    }
}