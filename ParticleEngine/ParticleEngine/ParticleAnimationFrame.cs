using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class ParticleAnimationFrame
    {
        public TimeSpan Duration { get; set; }
        
        //public Color StartColor { get; set; }
        //public Color EndColor { get; set; }

        //public Vector2 StartDirection { get; set; }
        //public Vector2 EndDirection { get; set; }

        public double StartVelocity { get; set; }
        public double EndVelocity { get; set; }
    }
}
