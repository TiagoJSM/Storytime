using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.Puppeteer;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class TimeLineTuple
    {
        public BoneViewModel Bone { get; set; }

        public SingleTimeLineControl Control { get; set; }
    }
}
