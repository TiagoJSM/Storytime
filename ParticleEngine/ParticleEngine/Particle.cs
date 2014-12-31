using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class Particle
    {
        private TimeSpan _timeToLive;
        //public Color Color { get; set; }

        public TimeSpan TimeToLive
        {
            get { return _timeToLive; }
            set
            {
                if (_timeToLive.Ticks == value.Ticks) return;
                _timeToLive = value;
                if (TimeToLive.Ticks <= 0)
                {
                    //Kill();
                }
            }
        }

        public ParticleAnimationBoard AnimationBoard { get; set; }

        public void Update(double elapsed)
        {
            TimeToLive = TimeToLive.Subtract(TimeSpan.FromMilliseconds(elapsed));
            if (TimeToLive.Ticks <= 0) return;
        }
    }
}
