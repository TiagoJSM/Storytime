using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Components;

namespace StoryTimeFramework.Entities.Actors
{
    public class ParticleEmitterActor : BaseActor
    {
        public ParticleEmitterComponent ParticleEmitterComponent { get; private set; }
       
        public ParticleEmitterActor()
        {
            OnCreated += OnCreatedHandler;
        }

        private void OnCreatedHandler()
        {
            Body = Scene.PhysicalWorld.CreateRectangularBody(1, 1, 1);
            ParticleEmitterComponent = Components.AddComponent<ParticleEmitterComponent>();
        }
    }
}
