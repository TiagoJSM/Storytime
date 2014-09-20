﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Controls.Puppeteer;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface ISkeletonViewerController : IController<ISkeletonTreeViewControl>
    {
        ISkeletonTreeViewControl SkeletonTreeViewControl { get; set; }
    }
}
