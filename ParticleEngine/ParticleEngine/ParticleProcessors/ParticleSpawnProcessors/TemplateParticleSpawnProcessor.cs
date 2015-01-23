using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.CustomAttributes.Editor;

namespace ParticleEngine.ParticleProcessors.ParticleSpawnProcessors
{
    public abstract class TemplateParticleSpawnProcessor : ParticleSpawnProcessor
    {
        private double _totalElapsedSinceLastParticleInMillis;
        private double _emissionRateInMilliseconds;

        [Editable(EditorName = "Emission rate in milliseconds")]
        public double EmissionRateInMilliseconds
        {
            get { return _emissionRateInMilliseconds; }
            set
            {
                if (_emissionRateInMilliseconds == value) return;
                if (value <= 0) return;
                _emissionRateInMilliseconds = value;
            }
        }

        public int? MaxParticles { get; set; }

        public TemplateParticleSpawnProcessor(ParticleEmitter emitter)
            : base(emitter)
        { 
        }

        public override void TimeElapse(TimeSpan elapsedSinceLastUpdate)
        {
            _totalElapsedSinceLastParticleInMillis += elapsedSinceLastUpdate.TotalMilliseconds;

            while (_totalElapsedSinceLastParticleInMillis > EmissionRateInMilliseconds)
            {
                _totalElapsedSinceLastParticleInMillis -= EmissionRateInMilliseconds;
                if (MaxParticles != null && MaxParticles.Value >= ParticleEmitter.SpawnedParticlesCount)
                    continue;

                DoTimeElapse(elapsedSinceLastUpdate);
            }
        }

        protected abstract void DoTimeElapse(TimeSpan elapsedSinceLastUpdate);
    }
}
