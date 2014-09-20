using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneViewModel : TreeViewItemViewModel
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public BoneViewModel(INodeAddedCallback nodeAddCB)
            :base(nodeAddCB, false)
        {
        }
    }
}
