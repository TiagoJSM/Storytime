using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeLineTool;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Extensions
{
    public static class ITimeLineDataItemExtensions
    {
        public static bool IsIntervalIntesected(this TimeFrame item, TimeSpan time, bool includeEnding = false)
        {
            if (item.StartTime == null && item.EndTime == null) return true;

            if (item.StartTime == null)
            {
                return item.IsEndTimeMoreThan(time, includeEnding);
            }

            if (item.EndTime == null)
            {
                return item.IsStartTimeLessThan(time);
            }

            return item.IsEndTimeMoreThan(time, includeEnding) && item.IsStartTimeLessThan(time);
        }

        private static bool IsEndTimeMoreThan(this TimeFrame item, TimeSpan time, bool includeEnding = false)
        {
            if(includeEnding)
                return item.EndTime >= time;
            return item.EndTime > time;
        }

        private static bool IsStartTimeLessThan(this TimeFrame item, TimeSpan time)
        {
            return item.StartTime <= time;
        }
    }
}
