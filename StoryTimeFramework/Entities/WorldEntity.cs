using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.General;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.WorldManagement;
using StoryTimeCore.Extensions;

namespace StoryTimeFramework.Entities
{
    /// <summary>
    /// The base class for the entities that exist in the world, by inheriting this class it's possible
    /// to receive inputs from the engine
    /// </summary>
    public abstract class WorldEntity : IBoundingBoxable
    {
        private bool _initialized;
        public event Action OnCreated;
        public event Action OnDestroyed;

        public GameWorld World { get; set; }
        public Scene Scene { get; set; }

        public event Action<WorldEntity> OnBoundingBoxChanges;

        public abstract AxisAlignedBoundingBox2D AABoundingBox { get; }
        public abstract BoundingBox2D BoundingBox { get; }

        public void Initialize()
        {
            if (_initialized) return;
            if (OnCreated != null)
                OnCreated();
            _initialized = true;
        }

        public void Destroy()
        {
            Scene.RemoveWorldEntity(this);
            if (OnDestroyed != null)
                OnDestroyed();
        }

        public abstract void TimeElapse(WorldTime WTime);

        protected void RaiseBoundingBoxChanges()
        {
            if (OnBoundingBoxChanges != null)
                OnBoundingBoxChanges(this);
        }
    }
}
