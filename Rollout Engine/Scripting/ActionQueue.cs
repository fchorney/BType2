using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting
{
    
    public class ActionQueue : List<IAction>
    {
        private List<IAction> queue; 
        public List<IAction> Queue { 
            get { return queue ?? (queue = new List<IAction>()); }
        }

        public new void Add(IAction action)
        {
            (this as List<IAction>).Add(action);
            Queue.Add(action);
        }

        public void Update(GameTime gameTime)
        {
            var finishedActions = new List<IAction>();

            foreach (var action in Queue)
            {
                action.Update(gameTime);
                if (action.Finished)
                {
                    finishedActions.Add(action);
                }
                if (action.Wait)
                {
                    break;
                }
            }

            foreach (var action in finishedActions)
            {
                Queue.Remove(action);
            }

          
        }

        public virtual void Reset()
        {
            foreach (var action in this)
            {
                action.Reset();
            }

            Queue.Clear();

            foreach (var action in this)
            {
                Queue.Add(action);
            }
        }
    }
}