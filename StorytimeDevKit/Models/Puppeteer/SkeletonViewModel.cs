using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class SkeletonViewModel : TreeViewItemViewModel
    {
        public SkeletonViewModel(INodeAddedCallback nodeAddCB)
            : base(null, nodeAddCB, false)
        {
        }
    }
}
