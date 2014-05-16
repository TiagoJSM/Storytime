using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.SceneViewer;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Commands;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public class SceneViewerController : ISceneViewerController
    {
        private ISceneViewerControl _control;
        private CommandStack _commands;

        public void AddActor(Scene s, BaseActor actor)
        {
            IReversibleCommand command = new AddActorCommand(s, actor);
            command.Run();
            _commands.Push(command);
        }

        public ISceneViewerControl Control
        {
            set { _control = value; }
        }
    }
}
