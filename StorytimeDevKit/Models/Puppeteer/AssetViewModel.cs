using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Controls;
using Puppeteer.Resources;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AssetViewModel : TreeViewItemViewModel
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

        public BoneAttachedRenderableAsset Asset { get; set; }

        public AssetViewModel(INodeAddedCallback nodeAddCB, string name)
            : this(null, nodeAddCB, name)
        {
        }

        public AssetViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, string name)
            : base(parent, nodeAddCB, false)
        {
            Name = name;
        }
    }
}
