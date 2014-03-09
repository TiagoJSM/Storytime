using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Exceptions;

namespace StoryTimeCore.WorldManagement
{
    public class GraphicsContextConfigurationException : StoryTimeException
    {
        private const string _message = "The GraphicsContext can only be configured once.";
        public GraphicsContextConfigurationException() : base(_message) { }
    }

    public class World
    {
        public static World _singleton;
        public static World Singleton 
        { 
            get 
            {
                if(_singleton == null)
                    _singleton = new World();
                return _singleton; 
            } 
        }

        private List<Scene> _scenes;
        private IGraphicsContext _graphicsContext;

        private World() 
        {
            _scenes = new List<Scene>();
        }

        public IGraphicsContext GraphicsContext
        { 
            get 
            {
                return _graphicsContext;
            } 
            set
            {
                if (_graphicsContext != null)
                    throw new GraphicsContextConfigurationException();
                _graphicsContext = value;
            } 
        }

        public int NumberOfScenes { get { return _scenes.Count; } }
        public Scene GetSceneAt(int index) { return _scenes[index]; }

        public void Render()
        {
            foreach (Scene scene in _scenes)
                scene.Render(_graphicsContext);
        }
    }
}
