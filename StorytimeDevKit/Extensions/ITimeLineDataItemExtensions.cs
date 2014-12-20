using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeLineTool;

namespace StoryTimeDevKit.Extensions
{
    public static class ITimeLineDataItemExtensions
    {
        public static bool IsIntervalIntesected(this ITimeLineDataItem item, DateTime time)
        {
            if (item.StartTime == null && item.EndTime == null) return true;

            if (item.StartTime == null)
            {
                return item.IsEndTimeMoreThan(time);
            }

            if (item.EndTime == null)
            {
                return item.IsStartTimeLessThan(time);
            }

            return item.IsEndTimeMoreThan(time) && item.IsStartTimeLessThan(time);
        }

        private static bool IsEndTimeMoreThan(this ITimeLineDataItem item, DateTime time)
        {
            return item.EndTime.Value > time;
        }

        private static bool IsStartTimeLessThan(this ITimeLineDataItem item, DateTime time)
        {
            return item.StartTime.Value <= time;
        }
    }
}
