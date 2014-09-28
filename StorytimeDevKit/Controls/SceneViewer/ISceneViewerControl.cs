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
    public interface ISceneViewerControl
    {
        event Action<BaseActor> OnSelectedActorChange;

        event OnDropActor OnDropActor;
        event OnMouseMove OnMouseMove;
        event OnMouseClick OnMouseClick;
        event OnMouseDown OnMouseDown;
        event OnMouseUp OnMouseUp;
        event OnSceneAdded OnSceneAdded;

        void AddScene(SceneViewModel s);
        void SaveSelectedScene();
        void Undo();
        void Redo();

        int CommandCount { get; }
        int? CommandIndex { get; }
        bool CanUndo { get; }
        bool CanRedo { get; }

        BaseActor SelectedActor { get; }
    }
}
