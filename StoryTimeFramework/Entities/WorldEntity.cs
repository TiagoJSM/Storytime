using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeFramework.Entities
{
    /// <summary>
    /// The base class for the entities that exist in the world, by inheriting this class it's possible
    /// to receive inputs from the engine
    /// </summary>
    public class WorldEntity
    {
        public event Action OnCreated;
        public event Action OnDestroyed;

        public GameWorld World { get; set; }
    }
}
