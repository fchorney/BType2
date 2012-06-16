using System;

namespace Rollout.Scripting
{
    public class ActionAttribute : Attribute
    {
        public string Name { get; set; }
        public ActionAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ActionParamAttribute : Attribute
    {
        public string Name { get; set; }

        public ActionParamAttribute(string name)
        {
            this.Name = name;
        }
    }
}