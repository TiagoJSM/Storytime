using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class ParticleAnimationBoard
    {
        public IEnumerable<ParticleAnimationFrame> Frames { get; set; }

        public ParticleAnimationFrame GetAt(TimeSpan time)
        {
            var frameElapsedTime = new TimeSpan();
            foreach (var frame in Frames)
            {
                frameElapsedTime = frameElapsedTime.Add(frame.Duration);
                if (frameElapsedTime.Ticks >= time.Ticks)
                    return frame;
            }
            return null;
        }
    }
}
