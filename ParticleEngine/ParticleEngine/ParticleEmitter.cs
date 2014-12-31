using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleEngine
{
    public class ParticleEmitter
    {
        private double _totalElapsedSinceLastParticleInMillis;
        private double _emissionRateInMilliseconds;
        private List<Particle> _spawnedParticles; 

        public string ParticlePath { get; set; }

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

        //public Vector2 EmissionDirection { get; set; }

        public double EmissionVelocity { get; set; }

        public bool Enabled { get; set; }

        public int? MaxParticles { get; set; }

        public TimeSpan ParticlesTimeToLive { get; set; }

        //public Vector2 ParticleSpawnOffsetPosition { get; set; }
        //public Vector2 ParticleSpawnOffsetDirection { get; set; }
        public double ParticleSpawnOffsetVelocity { get; set; }
        public double ParticleTimeToLiveOffsetInMilliseconds { get; set; }

        public ParticleAnimationBoard AnimationBoard { get; set; }

        public ParticleEmitter()
        {
            EmissionRateInMilliseconds = 1;
            Enabled = true;
            EmissionVelocity = 1;
            //EmissionDirection = new Vector(0, 1);
            _spawnedParticles = new List<Particle>();
        }

        public void Update(double elapsed)
        {
            if (!Enabled) return;

            _totalElapsedSinceLastParticleInMillis += elapsed;

            while (_totalElapsedSinceLastParticleInMillis > EmissionRateInMilliseconds)
            {
                _totalElapsedSinceLastParticleInMillis -= EmissionRateInMilliseconds;
                if (MaxParticles != null && MaxParticles.Value >= _spawnedParticles.Count)
                    continue;

                Particle particle = null;
                //spawn particle
                //assign destruction handler
                _spawnedParticles.Add(particle);
                OnParticleSpawned(particle);
            }
        }

        private void OnDestroyHandler()
        {
            
        }

        protected virtual void OnParticleSpawned(Particle particle)
        {
            
        }
    }
}
