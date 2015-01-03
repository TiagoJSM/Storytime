using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Puppeteer.Animation
{
    public class BoneAnimationFrame
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Vector2 Translation { get; set; }
        public float Rotation { get; set; }
    }
}
