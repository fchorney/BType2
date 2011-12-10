using System;
using System.Collections.Generic;

namespace Rollout.Scripting
{
    public class ActionInfo
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public List<ParamInfo> Params { get; set; }

        public ActionInfo()
        {
            Params = new List<ParamInfo>();
        }

        public class ParamInfo
        {
            public Type Type { get; set; }
            public string Name { get; set; }
            public object DefaultValue { get; set; }
            public int Order { get; set; }
        }
    }
}