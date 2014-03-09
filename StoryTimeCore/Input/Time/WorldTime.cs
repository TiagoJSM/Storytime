using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Input.Time
{
    /// <summary>
    /// The class that represents the time in the world in at a certain point.
    /// </summary>
    public class WorldTime
    {
        public WorldTime(TimeSpan totalElapsedTime, TimeSpan elapsedSinceLastTime)
        {
            _totalElapsedTime = totalElapsedTime;
        }

        private TimeSpan _totalElapsedTime;
        private TimeSpan _elapsedSinceLastTime;

        public TimeSpan TotalElapsedTime { get { return _totalElapsedTime; } }
        public TimeSpan ElapsedSinceLastTime { get { return _elapsedSinceLastTime; } }
    }
}
