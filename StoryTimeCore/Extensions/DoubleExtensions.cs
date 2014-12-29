using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class DoubleExtensions
    {
        public static double NearestMultipleOf(this double value, double multipleOf)
        {
            return Math.Round((value / multipleOf), MidpointRounding.AwayFromZero) * multipleOf;
        }
    }
}
