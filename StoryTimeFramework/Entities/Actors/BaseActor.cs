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
using StoryTimeCore.CustomAttributes.Editor;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene actors, this class defines the drawable components for the scene.
    /// </summary>
    public abstract class BaseActor : WorldEntity, ITimeUpdatable
    {
        [Editable(EditorGroup = "Physics", EditorName = "Body")]
        public IBody Body { get; set; }
        [Editable(EditorGroup = "Renderable", EditorName = "Actor")]
        public abstract IRenderableAsset RenderableActor { get; set; }
        public abstract void TimeElapse(WorldTime WTime);
    }
}
