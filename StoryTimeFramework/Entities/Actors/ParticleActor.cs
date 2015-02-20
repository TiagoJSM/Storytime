using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Components;
using StoryTimeFramework.Resources.Graphic;

namespace StoryTimeFramework.Entities.Actors
{
    public class ParticleActor : BaseActor
    {
        private Particle _particle;

        public Particle Particle
        {
            get { return _particle; }
            set
            {
                if (_particle == value) return;
                _particle = value;
                Body = _particle == null ? null : _particle.Body;
            }
        }

        public ParticleActor()
        {
            OnTimeElapse += OnTimeElapseHandler;
            OnCreated += OnOnCreatedHandler;
        }

        private void OnOnCreatedHandler()
        {
            Components.AddComponent<ParticleComponent>(c => c.Particle = _particle);
        }

        private void OnTimeElapseHandler(WorldTime wTime)
        {
            if (Particle == null) return;
            
            if(!Particle.IsAlive)
                Destroy();
        }
    }
}
