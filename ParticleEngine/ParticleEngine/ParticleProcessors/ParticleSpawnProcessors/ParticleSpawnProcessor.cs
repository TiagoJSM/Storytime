using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.CustomAttributes.Editor;

namespace ParticleEngine.ParticleProcessors.ParticleSpawnProcessors
{
    public abstract class ParticleSpawnProcessor
    {
        public ParticleEmitter ParticleEmitter { get; private set; }
    
        public ParticleSpawnProcessor(ParticleEmitter emitter)
        {
            ParticleEmitter = emitter;
        }

        public abstract void TimeElapse(TimeSpan elapsedSinceLastUpdate);
    }
}
