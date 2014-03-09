using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;

namespace StoryTimeCore.Manageables
{
    /// <summary>
    /// The interface used by the framework to pass the time at a certain instance.
    /// </summary>
    public interface ITimeUpdatable
    {
        void TimeElapse(WorldTime WTime);
    }
}
