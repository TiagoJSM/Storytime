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

        protected override void DoRender(IRenderer renderer)
        {
            renderer.RenderBoundingBox(this.BoundingBox, Color.Red, 1.0f);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox { get { return new AxisAlignedBoundingBox2D(0, 0, 50); } }

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
