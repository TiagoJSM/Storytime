using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{

    public class TextureViewModel : TreeViewItemViewModel
    {
        private string _name;
        private string _fullPath;

        public string Name { get { return _name; } }
        public string FullPath { get { return _fullPath; } }

        public TextureViewModel(TreeViewItemViewModel parent, string name, string fullpath)
            : base(parent, false)
        {
            _name = name;
            _fullPath = fullpath;
        }
        
    }
}
