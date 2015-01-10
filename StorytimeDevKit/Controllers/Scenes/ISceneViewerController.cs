using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.SceneViewer;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Models.SceneViewer;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.Renderables;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public interface ISceneViewerController : IStackedCommandsController<ISceneViewerControl>
    {
        ISceneViewerControl Control { set; }
        void AddActor(SceneTabViewModel s, ActorViewModel actor, Vector2 position);
        //void SelectWidget(ISceneWidget selected, ISceneWidget toSelect);
        void SaveScene(SceneTabViewModel scene);
    }
}
