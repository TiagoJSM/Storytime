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
using StoryTimeCore.Physics;

namespace StoryTimeFramework.Entities.Components
{
    public class ComponentCollection : IEnumerable<Component>
    {
        private List<Component> _components;
        private AxisAlignedBoundingBox2D _rawAABoundingBox;

        public BaseActor OwnerActor { get; set; }
        public Scene Scene { get { return OwnerActor.Scene; } }
        public IEnumerable<Component> Components { get { return _components; } }
        public BoundingBox2D BoundingBox 
        {
            get { return _rawAABoundingBox.GetBoundingBox2D(); }
        }
        public AxisAlignedBoundingBox2D AABoundingBox
        {
            get 
            {
                return _rawAABoundingBox;
            }
        }

        public ComponentCollection(BaseActor ownerActor)
        {
            _components = new List<Component>();
            OwnerActor = ownerActor;
            ownerActor.OnBodyChanges += OnBodyChangesHandler;
            UpdateBoundingBox();
        }

        public TComponent AddComponent<TComponent>(Action<TComponent> initializer = null) where TComponent : Component
        {
            Action<TComponent> componentInitializer = c =>
            {
                c.Owner = OwnerActor;
                c.OnBoundingBoxChanges += OnBoundingBoxChangesHandler;
                if(initializer != null)
                    initializer(c);
            };
            var component = Scene.AddWorldEntity<TComponent>(componentInitializer);
            _components.Add(component);
            UpdateBoundingBox();
            return component;
        }

        public void Render(IRenderer renderer)
        {
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

        private void OnBoundingBoxChangesHandler(WorldEntity entity)
        {
            UpdateBoundingBox();
        }

        private void OnBodyChangesHandler(IBody body)
        {
            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            if (!Components.Any())
            {
                if (OwnerActor.Body == null)
                {
                    _rawAABoundingBox = new AxisAlignedBoundingBox2D();
                }
                else
                {
                    _rawAABoundingBox = _rawAABoundingBox = new AxisAlignedBoundingBox2D(OwnerActor.Body.Position);
                }
            }
            else
            {
                _rawAABoundingBox = Components.Select(c => c.AABoundingBox).Combine();
            } 
        }
    }
}
