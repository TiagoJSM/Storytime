using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class SceneViewModel : TreeViewItemViewModel
    {
        public string SceneName { get; private set; }
        public string FullPath { get; private set; }

        public SceneViewModel(TreeViewItemViewModel parent, IGameObjectsControl gameObjects, string sceneName, string fullpath)
            : base(parent, gameObjects, false)
        {
            SceneName = sceneName;
            FullPath = fullpath;
        }
    }
}
