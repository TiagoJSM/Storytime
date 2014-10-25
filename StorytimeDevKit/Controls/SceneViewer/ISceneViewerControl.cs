using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Delegates;

namespace StoryTimeDevKit.Controls.SceneViewer
{
    public interface ISceneViewerControl : IMouseInteractiveControl
    {
        event Action<BaseActor> OnSelectedActorChange;

        event OnDropActor OnDropActor;

        event OnSceneAdded OnSceneAdded;
        event OnSceneChanged OnSceneChanged;

        void AddScene(SceneViewModel s);
        void SaveSelectedScene();
        void Undo();
        void Redo();

        int CommandCount { get; }
        int? CommandIndex { get; }
        bool CanUndo { get; }
        bool CanRedo { get; }

        //BaseActor SelectedActor { get; }
    }
}
