using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class AssemblyViewModel : TreeViewItemViewModel
    {
        public string AssemblyName { get; private set; }

        public AssemblyViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, string assemblyName)
            : base(parent, nodeAddCB, false)
        {
            AssemblyName = assemblyName;
        }
    }
}
