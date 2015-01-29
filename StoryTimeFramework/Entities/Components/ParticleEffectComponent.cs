using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeFramework.Entities.Components
{
    public class ParticleEffectComponent : Component
    {
        public ParticleEffect ParticleEffect { get; private set; }

        public ParticleEffectComponent()
        {
            OnCreated += OnCreatedHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
            ParticleEffect.Position = OwnerActor.Body.Position;
            ParticleEffect.TimeElapse(WTime.ElapsedSinceLastTime);
        }

        private void OnCreatedHandler()
        {
            ParticleEffect = new ParticleEffect(Scene.PhysicalWorld);
            ParticleEffect.OnParticleEmitterSpawned += OnParticleEmitterSpawnedHandler;
        }

        private void OnParticleEmitterSpawnedHandler(ParticleEmitter emitter)
        {
            emitter.OnParticleSpawned += OnParticleSpawnedHandler;
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
