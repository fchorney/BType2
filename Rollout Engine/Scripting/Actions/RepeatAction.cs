using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Actions
{
    public class RepeatAction : Action, IAction
    {
        private int Iterations { get; set; }
        private int CurrentIterations { get; set; }


        public RepeatAction(int n)
        {
            Iterations = n;
            CurrentIterations = n;
        }

        public void Update(GameTime gameTime)
        {
            List<IAction> finishedActions = new List<IAction>();

            foreach (var action in actionQueue)
            {
                action.Update(gameTime);
                if (action.Finished)
                {
                    finishedActions.Add(action);
                }
                if (action is WaitAction)
                {
                    break;
                }
            }

            foreach (var action in finishedActions)
            {
                actionQueue.Remove(action);
            }

            if (actionQueue.Count == 0)
            {
                if (CurrentIterations == 0)
                {
                    Finished = true;
                }
                else
                {
                    CurrentIterations--;
                    Reset();
                }
            }

        }

    }
}