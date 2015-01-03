using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneAnimationTimeFrame : TimeFrame
    {
        public BoneState StartState { get; set; }
        public BoneState EndState { get; set; }
    }
}
