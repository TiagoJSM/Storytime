using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Controls.SceneViewer
{
    public interface ISceneViewerControl
    {
        void AddScene(SceneViewModel s);
    }
}
