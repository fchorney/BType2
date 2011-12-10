using System;
using System.Collections.Generic;
using System.Linq;

namespace Rollout.Utility
{
    public static class AttributeHelper
    {
        public static IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit = false) 
            where TAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(TAttribute), inherit)
                   select t;
        }
    }
}
