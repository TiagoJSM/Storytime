using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Controllers;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene characters.
    /// These classes are controlled by controllers and updated by them, no ne else should interfere with these update events.
    /// </summary>
    public class Character : Actor
    {
        public BaseController Controller { get; set; }
    }
}
