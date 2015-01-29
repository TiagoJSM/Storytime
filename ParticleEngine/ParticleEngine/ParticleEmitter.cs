using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ParticleEngine.ParticleProcessors;
using ParticleEngine.ParticleProcessors.ParticleSpawnProcessors;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Physics;

namespace ParticleEngine
{
    public class ParticleEmitter
    {
        private List<Particle> _spawnedParticles;

        public event Action<Particle> OnParticleSpawned;

        public string ParticlePath { get; set; }

        public Vector2 EmissionDirection { get; set; }
        public float EmissionVelocity { get; set; }
        public bool Enabled { get; set; }
        //public int? MaxParticles { get; set; }
        public TimeSpan ParticlesTimeToLive { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 ParticleSize { get; set; }
        public string ParticleTexture { get; set; }

        public IParticleBodyFactory ParticleBodyFactory { get; private set; }
        public bool ParticlesArePhysicallySimulated { get; set; }
        public int SpawnedParticlesCount
        {
            get { return _spawnedParticles.Count; }
        }

        public ParticleSpawnProcessor SpawnProcessor { get; private set; }
        public List<IParticleProcessor> ParticleProcessors { get; private set; } 

        public ParticleEmitter(IParticleBodyFactory particleBodyFactory)
        {
            Enabled = true;
            EmissionVelocity = 50;
            EmissionDirection = new Vector2(1, 1);
            _spawnedParticles = new List<Particle>();
            ParticleBodyFactory = particleBodyFactory;
            ParticleSize = new Vector2(10);
            ParticlesTimeToLive = TimeSpan.FromSeconds(10);
            ParticleProcessors = new List<IParticleProcessor>();
            ParticleTexture = "Particle";
        }

        public void TimeElapse(TimeSpan elapsedSinceLastUpdate)
        {
            if (!Enabled) return;

            UpdateParticles(elapsedSinceLastUpdate);
            if (SpawnProcessor != null)
            {
                SpawnProcessor.TimeElapse(elapsedSinceLastUpdate);   
            }
        }

        public TSpawnProcessor SetParticleSpawnProcessor<TSpawnProcessor>() where TSpawnProcessor : ParticleSpawnProcessor
        {
            return SetParticleSpawnProcessor(typeof(TSpawnProcessor)) as TSpawnProcessor;
        }

        public ParticleSpawnProcessor SetParticleSpawnProcessor(Type spawnProcessorType)
        {
            if (spawnProcessorType.IsAssignableFrom(typeof (ParticleSpawnProcessor)))
            {
                return null;
            }
            SpawnProcessor = Activator.CreateInstance(spawnProcessorType, new[] { this }) as ParticleSpawnProcessor;
            return SpawnProcessor;
        }

        public Particle SpawnParticle()
        {
            var particle = new Particle(
                ParticleBodyFactory.CreateParticleBody(ParticlesArePhysicallySimulated, ParticleSize.X, ParticleSize.Y, 0.1f),
                ParticleTexture)
            {
                TimeToLive = ParticlesTimeToLive,
                Direction = EmissionDirection,
                Velocity = EmissionVelocity
            };
            _spawnedParticles.Add(particle);
            if (OnParticleSpawned != null)
                OnParticleSpawned(particle);
            return particle;
        }

        private void UpdateParticles(TimeSpan elapsedSinceLastUpdate)
        {
            foreach (var particle in _spawnedParticles.ToList())
            {
                particle.ElapsedLifeTime = particle.ElapsedLifeTime.Add(elapsedSinceLastUpdate);
                if (!particle.IsAlive)
                {
                    _spawnedParticles.Remove(particle);
                }
            }

            foreach (var processor in ParticleProcessors)
            {
                processor.Process(_spawnedParticles);
            }
        }

        private void OnDestroyHandler()
        {
            
        }
    }
}
