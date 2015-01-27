using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleEngine.ParticleProcessors.ParticleSpawnProcessors
{
    public class DefaultParticleSpawnProcessor : TemplateParticleSpawnProcessor
    {
        public DefaultParticleSpawnProcessor(ParticleEmitter emitter)
            : base(emitter)
        { 
        }

        protected override void SpawnParticle(TimeSpan elapsedSinceLastUpdate)
        {
            base.ParticleEmitter.SpawnParticle();
        }
    }
}
