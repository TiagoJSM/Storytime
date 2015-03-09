using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class FloatExtensions
    {
        private const float MaxAngle = 360.0f;
        private const float MinAngle = 0.0f;

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
            var result = value;
            for (var idx = 1; idx < power; idx++)
                result *= value;
            return result;
        }

        public static float ReduceRotationToOneTurn(this float rotation)
        {
            if(rotation > MaxAngle && rotation < MinAngle)
            {
                return rotation;
            }
            if (rotation > MaxAngle)
            {
                while (rotation > MaxAngle)
                {
                    rotation -= MaxAngle;
                }
                return rotation;
            }
            while (rotation < MinAngle)
            {
                rotation += MaxAngle;
            }
            return rotation;
        }
    }
}
