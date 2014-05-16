using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class FolderViewModel : TreeViewItemViewModel
    {
        public string FolderName { get; private set; }
        public string FolderFullPath { get; private set; }

        public FolderViewModel(TreeViewItemViewModel parent, string folderName, string folderFullPath)
            : base(parent, false)
        {
            FolderName = folderName;
            FolderFullPath = folderFullPath;
        }
    }
}
