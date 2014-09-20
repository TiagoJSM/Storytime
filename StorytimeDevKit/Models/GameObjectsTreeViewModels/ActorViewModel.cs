using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ActorViewModel : TreeViewItemViewModel
    {
        public string ActorName { get; private set; }
        public string AssemblyName { get; private set; }
        public Type ActorType { get; private set; }

        public ActorViewModel(TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB, Type t, string assemblyName)
            : base(parent, nodeAddCB, false)
        {
            ActorType = t;
            ActorName = t.Name;
            AssemblyName = assemblyName;
        }
    }
}
