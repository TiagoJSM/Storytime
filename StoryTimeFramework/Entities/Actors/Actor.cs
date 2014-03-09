using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeCore.Entities.Actors
{
    public class Actor : BaseActor
    {
        public override IRenderableAsset RenderableActor { get; set; }
        public override void TimeElapse(WorldTime WTime)
        {
            RenderableActor.TimeElapse(WTime);
        }
    }
}
