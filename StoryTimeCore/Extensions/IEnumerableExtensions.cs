using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;

namespace StoryTimeCore.Extensions
{
    public static class IEnumerableExtensions
    {
        public static TVal MaxOrDefaultBy<TVal, Key>(this IEnumerable<TVal> vals, Func<TVal, Key> selector)
        {
            if (vals.Count() == 0) return default(TVal);
            return vals.MaxBy(selector);
        }

        public static TVal MinOrDefaultBy<TVal, Key>(this IEnumerable<TVal> vals, Func<TVal, Key> selector)
        {
            if (vals.Count() == 0) return default(TVal);
            return vals.MinBy(selector);
        }
    }
}
