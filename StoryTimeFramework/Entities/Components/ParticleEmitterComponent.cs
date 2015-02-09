using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeFramework.Entities.Components
{
    public class ParticleEmitterComponent : Component
    {
        public ParticleEmitter ParticleEmitter { get; private set; }

        public ParticleEmitterComponent()
        {
            OnCreated += OnCreatedHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
            ParticleEmitter.Position = OwnerActor.Body.Position;
            ParticleEmitter.TimeElapse(WTime.ElapsedSinceLastTime);
        }

        private void OnCreatedHandler()
        {
            ParticleEmitter = new ParticleEmitter(Scene.PhysicalWorld);
            ParticleEmitter.OnParticleSpawned += OnParticleSpawnedHandler;
        }

        private void OnParticleSpawnedHandler(Particle particle)
        {
            Scene.AddWorldEntity<ParticleActor>(pa =>
            {
                pa.Particle = particle;
            });
        }
    }
}
