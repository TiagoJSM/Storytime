using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.GameObjects;

namespace StoryTimeDevKit.Models.GameObjectsTreeViewModels
{
    public class AssemblyViewModel : TreeViewItemViewModel
    {
        public string AssemblyName { get; private set; }

        public AssemblyViewModel(TreeViewItemViewModel parent, IGameObjectsControl gameObjects, string assemblyName)
            : base(parent, gameObjects, false)
        {
            AssemblyName = assemblyName;
        }
    }
}
