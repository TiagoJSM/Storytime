using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.SceneObjects;

namespace StoryTimeDevKit.DataStructures.Factories
{
    public class TypeConfigurableSceneObjectFactory : ISceneObjectFactory
    {
        private Dictionary<Type, Action<ISceneObject>> _configurationsMapper;

        protected Dictionary<Type, Func<object, ISceneObject>> SceneObjectMapper { get; set; }

        public TypeConfigurableSceneObjectFactory()
        {
            SceneObjectMapper = new Dictionary<Type, Func<object, ISceneObject>>();
            _configurationsMapper = new Dictionary<Type, Action<ISceneObject>>();
        }

        public ISceneObject CreateSceneObject(object data)
        {
            var dataType = data.GetType();
            
            if(!SceneObjectMapper.ContainsKey(dataType)) return null;
            var sceneObject = SceneObjectMapper[dataType](data);

            if (_configurationsMapper.ContainsKey(dataType))
            {
                _configurationsMapper[dataType](sceneObject);
            }

            return sceneObject;
        }

        public TypeConfigurableSceneObjectFactory Bind<TData>(Action<ISceneObject> configuration)
        {
            var dataType = typeof(TData);
            if (!_configurationsMapper.ContainsKey(dataType))
            {
                _configurationsMapper.Remove(dataType);
            }
            _configurationsMapper.Add(dataType, configuration);
            return this;
        }
    }
}
