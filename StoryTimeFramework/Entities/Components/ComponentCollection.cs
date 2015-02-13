using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.WorldManagement;
using StoryTimeCore.General;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;
using StoryTimeCore.Contexts.Interfaces;

namespace StoryTimeFramework.Entities.Components
{
    public class ComponentCollection : Component, IEnumerable<Component>
    {
        private List<Component> _components;
        private AxisAlignedBoundingBox2D _rawAABoundingBox;
        
        public Scene Scene { get { return OwnerActor.Scene; } }
        public IEnumerable<Component> Components { get { return _components; } } 

        public ComponentCollection(BaseActor ownerActor)
        {
            _components = new List<Component>();
            OwnerActor = ownerActor;
        }

        public TComponent AddComponent<TComponent>(Action<TComponent> initializer = null) where TComponent : Component
        {
            Action<TComponent> componentInitializer = c =>
            {
                c.OwnerActor = OwnerActor;
                c.OnBoundingBoxChanges += OnBoundingBoxChangesHandler;
                if(initializer != null)
                    initializer(c);
            };
            var component = Scene.AddWorldEntity<TComponent>(componentInitializer);
            _components.Add(component);
            UpdateBoundingBox();
            return component;
        }

        public override void Render(IRenderer renderer)
        {
            if (!RenderInGame) return;
            foreach (var component in _components)
                component.Render(renderer);
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return _rawAABoundingBox; }
        }

        private void OnBoundingBoxChangesHandler(WorldEntity entity)
        {
            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            _rawAABoundingBox = Components.Select(c => c.AABoundingBox).Combine();
        }
    }
}
