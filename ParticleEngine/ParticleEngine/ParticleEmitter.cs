using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Physics;

namespace ParticleEngine
{
    public class ParticleEmitter
    {
        private double _totalElapsedSinceLastParticleInMillis;
        private double _emissionRateInMilliseconds;
        private List<Particle> _spawnedParticles;

        public event Action<Particle> OnParticleSpawned;

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
        public Vector2 EmissionDirection { get; set; }
        public float EmissionVelocity { get; set; }
        public bool Enabled { get; set; }
        public int? MaxParticles { get; set; }
        public TimeSpan ParticlesTimeToLive { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 ParticleSize { get; set; }

        public Vector2 ParticleSpawnOffsetPosition { get; set; }
        public Vector2 ParticleSpawnOffsetDirection { get; set; }
        public double ParticleSpawnOffsetVelocity { get; set; }
        public double ParticleTimeToLiveOffsetInMilliseconds { get; set; }
        public Vector2 ParticleSpawnOffsetSize { get; set; }

        public ParticleAnimationBoard AnimationBoard { get; set; }

        public IParticleBodyFactory ParticleBodyFactory { get; private set; }

        public bool ParticlesArePhysicallySimulated { get; set; }

        public ParticleEmitter(IParticleBodyFactory particleBodyFactory)
        {
            EmissionRateInMilliseconds = 1;
            Enabled = true;
            EmissionVelocity = 1;
            EmissionDirection = new Vector2(0, 1);
            _spawnedParticles = new List<Particle>();
            ParticleBodyFactory = particleBodyFactory;
        }

        public void TimeElapse(TimeSpan elapsedSinceLastUpdate)
        {
            if (!Enabled) return;

            _totalElapsedSinceLastParticleInMillis += elapsedSinceLastUpdate.TotalMilliseconds;

            UpdateParticles(elapsedSinceLastUpdate);

            while (_totalElapsedSinceLastParticleInMillis > EmissionRateInMilliseconds)
            {
                _totalElapsedSinceLastParticleInMillis -= EmissionRateInMilliseconds;
                if (MaxParticles != null && MaxParticles.Value >= _spawnedParticles.Count)
                    continue;

                var particle = SpawnParticle();
            }
        }

        private void UpdateParticles(TimeSpan elapsedSinceLastUpdate)
        {
            foreach (var particle in _spawnedParticles)
            {
                particle.ElapsedLifeTime = particle.ElapsedLifeTime.Add(elapsedSinceLastUpdate);
                if (!particle.IsAlive)
                {
                    _spawnedParticles.Remove(particle);
                    continue;
                }

                var frame = AnimationBoard.GetAt(particle.ElapsedLifeTime);
                particle.Color = frame.GetColorAt(particle.ElapsedLifeTime);
                particle.Velocity  = frame.GetVelocityAt(particle.ElapsedLifeTime);
                particle.Direction = frame.GetDirectionAt(particle.ElapsedLifeTime);
            }
        }

        private Particle SpawnParticle()
        {
            var particle = new Particle(
                ParticleBodyFactory.CreateParticleBody(ParticlesArePhysicallySimulated, ParticleSize.X, ParticleSize.Y, 0.1f))
            {
                TimeToLive = ParticlesTimeToLive
            };
            _spawnedParticles.Add(particle);
            if (OnParticleSpawned != null)
                OnParticleSpawned(particle);
            return particle;
        }

        private void OnDestroyHandler()
        {
            
        }
    }
}
