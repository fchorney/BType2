using Microsoft.Xna.Framework;

namespace Rollout.Scripting.Actions
{
    [Action("action")]    
    public class Action : IAction
    {        
        protected ActionQueue actions;
        public ActionQueue Actions
        {
            get { return actions ?? (actions = new ActionQueue()); }
        }

        public bool Wait { get; set; }

        public Action(bool wait = false)
        {
            Wait = wait;
            Finished = false;
            Actions.Reset();
        }

        public Action(string target) : this(false)
        {
        }

        public void AddAction(IAction action)
        {
            Actions.Add(action);
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