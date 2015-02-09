using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeFramework.Entities.Components
{
    public class ComponentCollection : IEnumerable<Component>
    {
        private List<Component> _components;
 
        public BaseActor ActorOwner { get; private set; }
        public Scene Scene { get { return ActorOwner.Scene; }}
        public IEnumerable<Component> Components { get { return _components; } } 

        public ComponentCollection(BaseActor ownerActor)
        {
            _components = new List<Component>();
            ActorOwner = ownerActor;
        }

        public TComponent AddComponent<TComponent>() where TComponent : Component
        {
            var component = Scene.AddWorldEntity<TComponent>();
            component.OwnerActor = ActorOwner;
            _components.Add(component);
            return component;
        }

        public IEnumerator<Component> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        public void TimeElapse(WorldTime WTime)
        {
            foreach (var component in _components)
            {
                component.TimeElapse(WTime);
            }
        }
    }
}
