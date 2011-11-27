using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Rollout.Utility.Cloning
{
    public static class Cloner
    {
        public static T Clone<T>(this T obj)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                T temp = (T)formatter.Deserialize(buffer);
                return temp;
            }
        }
    }
}
