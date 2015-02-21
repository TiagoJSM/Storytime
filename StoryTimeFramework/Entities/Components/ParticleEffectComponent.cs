using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Contexts.Interfaces;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

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
            ParticleEffect.Position = Owner.Transformation.Translation.ToVector2();
            ParticleEffect.TimeElapse(WTime.ElapsedSinceLastTime);
        }

        protected override void DoRender(IRenderer renderer)
        {
            renderer.RenderBoundingBox(this.BoundingBox, Color.Red, 1.0f);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox { get { return new AxisAlignedBoundingBox2D(0, 0, 50); } }

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
