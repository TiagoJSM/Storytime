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

namespace StoryTimeDevKit.Controllers.Scenes
{
    public interface ISceneViewerController : IController<ISceneViewerControl>
    {
        void AddActor(SceneTabViewModel s, ActorViewModel actor, Vector2 position);
        void Undo();
        void Redo();

        int CommandCount { get; }
        int? CommandIndex { get; }
        bool CanUndo { get; }
        bool CanRedo { get; }
    }
}
