using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Drawing;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{
    [Action("action")]    
    public class Action : IAction
    {
        protected Dictionary<string, Expression> Args;

        protected ActionQueue actions;
        public ActionQueue Actions
        {
            get { return actions ?? (actions = new ActionQueue()); }
        }

        public bool Wait { get; set; }

        protected string SourceId { get; set; }
        protected ITransformable source { get; set; }
        protected ITransformable Source
        {
            get { return source ?? (source = ScriptingEngine.Item(SourceId) as ITransformable); }
        }

        public Action(bool wait = false)
        {
            Args = new Dictionary<string, Expression>();
            Wait = wait;
            Finished = false;
            Actions.Reset();
        }

        public Action(Dictionary<string, Expression> args)
            : this(false)
        {
            this.Args = args;
            SourceId = Args["source"].AsString();
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