using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;

namespace StoryTimeFramework.Entities.Controllers
{
    /// <summary>
    /// The base class for the AI Controllers.
    /// With this class the AI can take control of the actors and perform actions depending on the programmed decisions.
    /// </summary>
    public abstract class BaseAIController : BaseController
    {
        public override void TimeElapse(WorldTime WTime)
        {
            base.TimeElapse(WTime);
            PerformDecision(WTime);
        }

        protected virtual void PerformDecision(WorldTime WTime) { }
    }
}
