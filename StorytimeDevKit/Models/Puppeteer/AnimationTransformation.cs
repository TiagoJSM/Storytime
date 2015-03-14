using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Puppeteer.Animation;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AnimationTransformation
    {
        public Vector2 StartPosition { get; set; }
        public float StartRotation { get; set; }
        public Vector2 EndPosition { get; set; }
        public float EndRotation { get; set; }
        public bool ClockwiseRotation { get; set; }
    }
}
