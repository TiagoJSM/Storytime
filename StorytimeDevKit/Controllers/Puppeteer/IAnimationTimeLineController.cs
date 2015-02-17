using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface IAnimationTimeLineController : IController<IAnimationTimeLineControl>
    {
        IAnimationTimeLineControl TimeLineControl { get; set; }
        TimeSpan AnimationTotalTime { get; }
    }
}
