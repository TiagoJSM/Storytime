using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class ActorViewModel : TreeViewItemViewModel
    {

        public string ActorName { get; private set; }
        public string AssemblyName { get; private set; }

        public ActorViewModel(TreeViewItemViewModel parent, IGameObjectsControl gameObjects, string actorName, string assemblyName)
            : base(parent, gameObjects, false)
        {
            ActorName = actorName;
            AssemblyName = assemblyName;
        }
    }
}
