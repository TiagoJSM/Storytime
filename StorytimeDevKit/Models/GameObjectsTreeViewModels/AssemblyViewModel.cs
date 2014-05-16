using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class AssemblyViewModel : TreeViewItemViewModel
    {
        public string AssemblyName { get; private set; }

        public AssemblyViewModel(TreeViewItemViewModel parent, string assemblyName)
            : base(parent, false)
        {
            AssemblyName = assemblyName;
        }
    }
}
