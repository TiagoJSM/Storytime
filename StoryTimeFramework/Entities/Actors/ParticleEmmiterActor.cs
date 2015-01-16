using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Components;

namespace StoryTimeFramework.Entities.Actors
{
    public class ParticleEmmiterActor : BaseActor
    {
        public ParticleEmitterComponent ParticleEmitterComponent { get; private set; }
       
        public ParticleEmmiterActor()
        {
            OnCreated += OnCreatedHandler;
        }

        private void OnCreatedHandler()
        {
            ParticleEmitterComponent = Components.AddComponent<ParticleEmitterComponent>();
        }
    }
}
