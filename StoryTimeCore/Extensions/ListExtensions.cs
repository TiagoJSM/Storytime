using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class ListExtensions
    {
        public static void Move<TData>(this IList<TData> list, TData data, int index)
        {
            if (!list.Contains(data)) return;
            list.Remove(data);
            list.Insert(index, data);
        }
    }
}
