using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class FolderViewModel : TreeViewItemViewModel
    {
        public string FolderName { get; private set; }
        public string FolderFullPath { get; private set; }

        public FolderViewModel(
            TreeViewItemViewModel parent,
            INodeAddedCallback nodeAddCB, 
            string folderName, 
            string folderFullPath, 
            string tag = null)
            : base(parent, nodeAddCB, false)
        {
            FolderName = folderName;
            FolderFullPath = folderFullPath;
            Tag = tag;
        }
    }
}
