using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Components;

namespace StoryTimeFramework.Entities.Actors
{
    public class ParticleEffectActor: BaseActor
    {
        public ParticleEffectComponent ParticleEffectComponent { get; private set; }

        public ParticleEffectActor()
        {
            OnCreated += OnCreatedHandler;
        }

        private void OnCreatedHandler()
        {
            Body = Scene.PhysicalWorld.CreateRectangularBody(1, 1, 1);
            ParticleEffectComponent = Components.AddComponent<ParticleEffectComponent>();
        }
    }
}
