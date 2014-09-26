using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class FloatExtensions
    {
        public static float BiggerOrEqualThan(this float f1, float f2)
        {
            if (f1 >= f2)
                return f1;
            return f2;
        }

        public static float SmallerOrEqualThan(this float f1, float f2)
        {
            if (f1 >= f2)
                return f2;
            return f1;
        }

        public static float PowerOf(this float value, int power)
        {
            float result = value;
            for (int idx = 1; idx < power; idx++)
                result *= value;
            return result;
        }
    }
}
