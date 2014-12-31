using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Extensions
{
    public static class ListExtensions
    {
        public static int? LastIndexOf<TSearch, TData>(this IList<TData> list) where TSearch : TData
        {
            var t = typeof(TSearch);
            var count = list.Count;

            for (var idx = count - 1; idx > 0; idx--)
            {
                if (list[idx].GetType() == t)
                    return idx;
            }
            return null;
        }
    }
}
