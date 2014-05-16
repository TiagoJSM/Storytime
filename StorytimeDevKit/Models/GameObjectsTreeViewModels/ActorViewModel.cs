using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ActorViewModel : TreeViewItemViewModel
    {

        public string ActorName { get; private set; }
        public string AssemblyName { get; private set; }

        public ActorViewModel(TreeViewItemViewModel parent, string actorName, string assemblyName)
            : base(parent, false)
        {
            ActorName = actorName;
            AssemblyName = assemblyName;
        }
    }
}
