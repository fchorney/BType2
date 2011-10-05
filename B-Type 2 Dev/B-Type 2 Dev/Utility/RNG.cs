using System;

namespace B_Type_2_Dev.Utility
{
    static class RNG
    {
        private static Random r = new Random();

        public static int Next()
        {
            return r.Next();
        }

        public static int Next(int maxValue)
        {
            return r.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }
    }
}
