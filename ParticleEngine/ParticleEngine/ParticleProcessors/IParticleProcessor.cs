using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleEngine.ParticleProcessors
{
    public interface IParticleProcessor
    {
        void TimeElapse(TimeSpan elapsedSinceLastUpdate);
        void Process(IEnumerable<Particle> particles);
    }
}
