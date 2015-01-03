using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeFramework.Entities.Components
{
    public abstract class Component : WorldEntity
    {
        public BaseActor OwnerActor { get; set; }
    }
}
