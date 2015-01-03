using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeCore.Input.Time;

namespace StoryTimeFramework.Entities.Actors
{
    public class ParticleActor : BaseActor
    {
        public Particle Particle { get; set; }

        public override void TimeElapse(WorldTime WTime)
        {
        }
    }
}
