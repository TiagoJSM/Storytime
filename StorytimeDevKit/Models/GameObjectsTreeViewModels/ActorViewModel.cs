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
        public Type ActorType { get; private set; }

        public ActorViewModel(TreeViewItemViewModel parent, IGameObjectsControl gameObjects, Type t, string assemblyName)
            : base(parent, gameObjects, false)
        {
            ActorType = t;
            ActorName = t.Name;
            AssemblyName = assemblyName;
        }
    }
}
