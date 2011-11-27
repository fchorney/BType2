using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Scripting.Actions;

namespace Rollout.Scripting
{
    [Serializable]
    public abstract class Action : IAction
    {        
        protected ActionQueue actions;
        public ActionQueue Actions
        {
            get { return actions ?? (actions = new ActionQueue()); }
        }

        public bool Wait { get; set; }

        protected IScriptingEngine engine;
        public IScriptingEngine Engine
        {
            get { return engine; }
            set { 
                engine = value;
                foreach (var action in Actions)
                {
                    action.Engine = engine;
                }
            }
        }

        public Action(bool wait = false)
        {
            Wait = wait;
            Reset();
        }



        public void AddAction(IAction action)
        {
            actions.Add(action);
        }

        [Obsolete("Dont need bool autowait anymore")]
        public void AddAction2(IAction action, bool autoWait = false)
        {
            if (!action.Wait)
            {
                action.Wait = autoWait;
            }
            actions.Add(action);
        }

        public virtual void Update(GameTime gameTime)
        {
            actions.Update(gameTime);
        }

        public virtual void Reset()
        {
            Finished = false;
            Actions.Reset();
        }

        public bool Finished { get; set; }
    }
}