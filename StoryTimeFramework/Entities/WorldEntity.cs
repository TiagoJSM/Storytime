using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeFramework.Entities
{
    /// <summary>
    /// The base class for the entities that exist in the world, by inheriting this class it's possible
    /// to receive inputs from the engine
    /// </summary>
    public class WorldEntity
    {
        protected event Action OnCreated;
        protected event Action OnDestroyed;
    }
}
