using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Exceptions;
using StoryTimeUI;

namespace StoryTimeFramework.WorldManagement
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
        private int _activeSceneIndex;

        public int NumberOfScenes { get { return _scenes.Count; } }
        public Scene ActiveScene 
        { 
            get 
            {
                if (_activeSceneIndex == NO_ACTIVE_SCENE) return null;
                return _scenes[_activeSceneIndex];
            } 
        }
        public IGraphicsContext GraphicsContext { get { return _graphicsContext; } }

        public GameWorld(IGraphicsContext graphicsContext) 
        {
            _scenes = new List<Scene>();
            _activeSceneIndex = NO_ACTIVE_SCENE;
            _graphicsContext = graphicsContext;
        }

        public Scene GetSceneAt(int index) { return _scenes[index]; }
        public void AddScene(Scene scene) 
        {
            scene.GraphicsContext = _graphicsContext;
            scene.Initialize();
            _scenes.Add(scene); 
        }

        public void RenderActiveScene()
        {
            if(_activeSceneIndex != NO_ACTIVE_SCENE)
                _scenes[_activeSceneIndex].Render(_graphicsContext);
            //TODO: should reset renderer here!
        }

        public void SetActiveScene(int index)
        {
            _activeSceneIndex = index;
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
