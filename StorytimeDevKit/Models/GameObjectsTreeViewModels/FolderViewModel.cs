using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class FolderViewModel : TreeViewItemViewModel
    {
        public string FolderName { get; private set; }
        public string FolderFullPath { get; private set; }

        public FolderViewModel(
            TreeViewItemViewModel parent, 
            IGameObjectsControl gameObjects, 
            string folderName, 
            string folderFullPath, 
            string tag = null)
            : base(parent, gameObjects, false)
        {
            FolderName = folderName;
            FolderFullPath = folderFullPath;
            Tag = tag;
        }
    }
}
