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
        private bool _dragOverTarget;

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

        public bool DragOverTarget
        {
            get
            {
                return _dragOverTarget;
            }
            set
            {
                if (_dragOverTarget == value) return;
                _dragOverTarget = value;
                OnPropertyChanged("DragOverTarget");
            }
        }

        public BoneAttachedRenderableAsset Asset { get; set; }

        public AssetViewModel(INodeAddedCallback nodeAddCB, BoneAttachedRenderableAsset asset, string name)
            : this(null, nodeAddCB, asset, name)
        {
        }

        public AssetViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, BoneAttachedRenderableAsset asset, string name)
            : base(parent, nodeAddCB, false)
        {
            Name = name;
            Asset = asset;
        }
    }
}
