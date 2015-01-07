using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Animation;
using MoreLinq;

namespace Puppeteer.Extensions
{
    public static class BoneAnimationFrameExtensions
    {
        public static BoneAnimationFrame GetAt(this IEnumerable<BoneAnimationFrame> frames, TimeSpan time)
        {
            return frames.FirstOrDefault(f => f.IsContainedInInterval(time));
        }

        public static BoneAnimationFrame GetLastFrame(this IEnumerable<BoneAnimationFrame> frames)
        {
            return frames.MaxBy(f => f.EndTime);
        }
    }
}
