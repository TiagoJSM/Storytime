using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.SceneViewer;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public interface ISceneViewerController : IController<ISceneViewerControl>
    {
        void AddActor(Scene s, BaseActor actor);
    }
}
