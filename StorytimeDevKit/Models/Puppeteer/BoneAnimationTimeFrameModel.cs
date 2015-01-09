using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Puppeteer.Animation;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneAnimationTimeFrameModel : TimeFrame
    {
        public BoneState StartState { get; set; }
        public BoneState EndState { get; set; }
        public BoneAnimationFrame AnimationFrame { get; set; }
    }
}
