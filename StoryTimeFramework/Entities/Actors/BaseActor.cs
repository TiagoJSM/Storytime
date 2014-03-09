using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Bodies;
using StoryTimeCore.Resources.Graphic;
using StoryTimeFramework.WorldManagement.Manageables;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.DataStructures;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeCore.DataStructures;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene actors, this class defines the drawable components for the scene.
    /// </summary>
    public abstract class BaseActor : WorldEntity, ITimeUpdatable
    {
        public IBody Body { get; set; }
        public abstract IRenderableAsset RenderableActor { get; set; }
        public abstract void TimeElapse(WorldTime WTime);
    }
}
