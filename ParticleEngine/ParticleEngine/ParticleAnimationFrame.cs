using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StoryTimeCore.Utils;

namespace ParticleEngine
{
    public class ParticleAnimationFrame
    {
        public TimeSpan Duration { get { return EndTime - StartTime; }}

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }

        public Vector2 StartDirection { get; set; }
        public Vector2 EndDirection { get; set; }

        public float StartVelocity { get; set; }
        public float EndVelocity { get; set; }

        public Color GetColorAt(TimeSpan totalElapsed)
        {
            return MathematicalUtils.LinearInterpolation(StartColor, EndColor, Duration, totalElapsed - StartTime);
        }

        public Vector2 GetDirectionAt(TimeSpan totalElapsed)
        {
            return MathematicalUtils.LinearInterpolation(StartDirection, EndDirection, Duration, totalElapsed - StartTime);
        }

        public float GetVelocityAt(TimeSpan totalElapsed)
        {
            return MathematicalUtils.LinearInterpolation(StartVelocity, EndVelocity, Duration, totalElapsed - StartTime);
        }
    }
}
