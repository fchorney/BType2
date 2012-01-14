using System;

namespace Rollout.Scripting
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SpriteAttribute : Attribute
    {
        public string Name { get; private set; }

        public SpriteAttribute(string name)
        {
            this.Name = name;
        }
    }
}