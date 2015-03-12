using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Utils
{
    public static class MathematicalUtils
    {
        public static float LinearInterpolation(float startValue, float endValue, TimeSpan totalTime, TimeSpan elapsedTime)
        {
            var valueDifference = endValue - startValue;
            var timeFraction = (float)elapsedTime.Ticks / (float)totalTime.Ticks;
            return startValue + valueDifference*timeFraction;
        }

        public static float LinearInterpolation(byte startValue, byte endValue, TimeSpan totalTime, TimeSpan elapsedTime)
        {
            var valueDifference = endValue - startValue;
            var timeFraction = elapsedTime.Ticks / totalTime.Ticks;
            return startValue + valueDifference * timeFraction;
        }

        public static Vector2 LinearInterpolation(Vector2 startValue, Vector2 endValue, TimeSpan totalTime, TimeSpan elapsedTime)
        {
            return new Vector2(
                LinearInterpolation(startValue.X, endValue.X, totalTime, elapsedTime), 
                LinearInterpolation(startValue.Y, endValue.Y, totalTime, elapsedTime));
        }

        public static Color LinearInterpolation(Color startValue, Color endValue, TimeSpan totalTime, TimeSpan elapsedTime)
        {
            return new Color(
                LinearInterpolation(startValue.R, endValue.R, totalTime, elapsedTime),
                LinearInterpolation(startValue.G, endValue.G, totalTime, elapsedTime),
                LinearInterpolation(startValue.B, endValue.B, totalTime, elapsedTime));
        }

        public static float CrossProduct(Vector2 a, Vector2 b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        public static bool IsClockwiseRotation(Vector2 a, Vector2 b, Vector2 center)
        {
            return ((a.X - center.X) * (b.Y - center.Y) - (a.Y - center.Y) * (b.X - center.X)) < 0;
        }

        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            var degrees =
                (float)Math.Acos(
                    CrossProduct(a, b) /
                    (a.Length() * b.Length())
                );
            return degrees;
        }

        public static float AngleBetween(Vector2 a, Vector2 b, Vector2 center)
        {
            return MathHelper.ToDegrees(AngleBetween(a - center, b - center));
        }
    }
}
