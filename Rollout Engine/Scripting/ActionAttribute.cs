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
        public Type Type { get; set; }
        public int Order { get; set; }

        public ActionParamAttribute(int order, string name, Type type)
        {
            this.Name = name;
            this.Type = type;
            this.Order = order;
        }
    }
}