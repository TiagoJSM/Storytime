using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleEngine.ParticleProcessors
{
    public abstract class TemplateParticleProcessor : IParticleProcessor
    {
        protected TimeSpan TotalElapsed { get; private set; }
        protected TimeSpan ElapsedSinceLastUpdate { get; private set; }

        public void TimeElapse(TimeSpan elapsedSinceLastUpdate)
        {
            ElapsedSinceLastUpdate = elapsedSinceLastUpdate;
            TotalElapsed = TotalElapsed + elapsedSinceLastUpdate;
        }

        public abstract void Process(IEnumerable<Particle> particles);
    }
}
