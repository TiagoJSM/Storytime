using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{

    public class TextureViewModel : TreeViewItemViewModel
    {
        private string _name;
        private string _fullPath;

        public string Name { get { return _name; } }
        public string FullPath { get { return _fullPath; } }

        public TextureViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, string name, string fullpath)
            : base(parent, nodeAddCB, false)
        {
            _name = name;
            _fullPath = fullpath;
        }
        
    }
}
