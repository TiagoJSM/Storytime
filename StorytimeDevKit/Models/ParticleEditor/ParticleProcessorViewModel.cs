using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleProcessorViewModel : ParticleTreeViewItem
    {
        public ICommand RemoveParticleProcessor { get; private set; }

        public ParticleProcessorViewModel(string name, TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB)
            : base(name, parent, nodeAddCB)
        {
        }
    }
}
