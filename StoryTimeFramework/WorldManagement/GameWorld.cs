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
        private int _activeScene;
        private GUIManager _gui;

        public GameWorld(IGraphicsContext graphicsContext) 
        {
            _scenes = new List<Scene>();
            _activeScene = NO_ACTIVE_SCENE;
            _graphicsContext = graphicsContext;
            _gui = new GUIManager(_graphicsContext);
        }

        public int NumberOfScenes { get { return _scenes.Count; } }
        public Scene GetSceneAt(int index) { return _scenes[index]; }
        public void AddScene(Scene scene) { _scenes.Add(scene); }
        public GUIManager GUI { get { return _gui; } }

        public void RenderActiveScene()
        {
            if(_activeScene != NO_ACTIVE_SCENE)
                _scenes[_activeScene].Render(_graphicsContext);
            //TODO: should reset renderer here!
            _gui.Render(_graphicsContext.GetRenderer());
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
