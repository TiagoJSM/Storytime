using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.WorldManagement.Manageables;

namespace StoryTimeFramework.Entities.Controllers
{
    /// <summary>
    /// The base class for the Controllers, this class contains utilitary methods and events for the derived controllers.
    /// </summary>
    public abstract class BaseController : WorldEntity, ITimeUpdatable
    {
        public Character Character { get; set; }
        protected event Action<WorldTime> OnTimeElapse;

        public virtual void TimeElapse(WorldTime WTime)
        {
            OnTimeElapse(WTime);
        }
    }
}
