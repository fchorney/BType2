using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Screens;
using Rollout.Scripting;

namespace Rollout.Scripting
{
    public static class ScriptingEngine
    {
        private static Dictionary<string, IScriptingEngine> Engines = new Dictionary<string, IScriptingEngine>();
        public static IScriptingEngine Engine
        {
            get
            {
                if (!Engines.ContainsKey(ScreenContext.ContextKey))
                    Engines.Add(ScreenContext.ContextKey, new XmlScriptingEngine());
                return Engines[ScreenContext.ContextKey];
            }
        }

        public static void Add(string name, IScriptable scriptable, List<IAction> actions = null)
        {
            Engine.Add(name, scriptable, actions);
        }

        public static void AddAction(string name, IAction action)
        {
            Engine.AddAction(name, action);
        }

        public static void Update(GameTime gameTime)
        {
            Engine.Update(gameTime);
        }

        public static IScriptable Item(string name)
        {
            return Engine[name];
        }
    }
}