using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class SceneViewModel : TreeViewItemViewModel
    {
        public string SceneName { get; private set; }
        public string Path { get; private set; }

        public SceneViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, string sceneName, string path)
            : base(parent, nodeAddCB, false)
        {
            SceneName = sceneName;
            Path = path;
        }
    }
}
