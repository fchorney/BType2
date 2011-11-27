using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Scripting.Actions;

namespace Rollout.Scripting
{
    public abstract class Action : IAction
    {        
        protected List<IAction> actions; 
        protected List<IAction> actionQueue;

        public bool Wait { get; set; }

        protected IScriptingEngine engine;
        public IScriptingEngine Engine
        {
            get { return engine; }
            set { engine = value; }
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

        }

        public virtual void Reset()
        {
            Finished = false;
            foreach (var action in Actions)
            {
                action.Reset();
            }

            ActionQueue.Clear();

            foreach (var action in Actions)
            {
                ActionQueue.Add(action);
            }
        }

        public bool Finished { get; set; }

        public List<IAction> Actions
        {
            get { return actions ?? (actions = new List<IAction>()); }
        }

        public List<IAction> ActionQueue
        {
            get { return actionQueue ?? (actionQueue = new List<IAction>()); }
        }
    }
}