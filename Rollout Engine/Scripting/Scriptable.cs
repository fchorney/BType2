namespace Rollout.Scripting
{
    class Scriptable
    {
        public string Name { get; set; }
        public IScriptable Object { get; set; }
        public ActionQueue Actions { get; set; } 

        public Scriptable()
        {
            Actions = new ActionQueue();
        }
    }
}