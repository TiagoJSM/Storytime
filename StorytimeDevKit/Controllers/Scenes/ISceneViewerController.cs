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
using StoryTimeDevKit.SceneWidgets;

namespace StoryTimeDevKit.Controllers.Scenes
{
    public interface ISceneViewerController : IStackedCommandsController<ISceneViewerControl>
    {
        void AddActor(SceneTabViewModel s, ActorViewModel actor, Vector2 position);
        void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition);
        void SelectWidget(ISceneWidget selected, ISceneWidget toSelect);
        void SaveScene(SceneTabViewModel scene);
    }
}
