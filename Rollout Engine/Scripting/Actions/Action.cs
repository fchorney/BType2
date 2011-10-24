using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Scripting.Actions;

namespace Rollout.Scripting
{
    public abstract class Action : IAction
    {

        protected List<IAction> actions; 
        protected List<IAction> actionQueue;

        public Action()
        {
            Reset();
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