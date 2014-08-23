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

    public class GameWorld
    {
        private const int NO_ACTIVE_SCENE = -1;

        private List<Scene> _scenes;
        private IGraphicsContext _graphicsContext;
        private int _activeScene;

        public GameWorld() 
        {
            _scenes = new List<Scene>();
            _activeScene = NO_ACTIVE_SCENE;
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
        public void AddScene(Scene scene) { _scenes.Add(scene); }

        public void RenderActiveScene()
        {
            if(_activeScene != NO_ACTIVE_SCENE)
                _scenes[_activeScene].Render(_graphicsContext);
        }

        public void SetActiveScene(int index)
        {
            _activeScene = index;
        }

        public void SetActiveScene(Scene s)
        {
            int index = _scenes.IndexOf(s);
            if (index == -1)
                return;
            SetActiveScene(index);
        }
    }
}
