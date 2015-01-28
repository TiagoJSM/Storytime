using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleEmitterViewModel : ParticleTreeViewItem
    {
        public ICommand SetParticleSpawnProcessor { get; private set; }
        public ICommand SetParticleProcessor { get; private set; }

        public ParticleEmitterViewModel(string name, INodeAddedCallback nodeAddCB)
            : this(name, null, nodeAddCB)
        {

        }

        public ParticleEmitterViewModel(string name, TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB)
            : base(name, parent, nodeAddCB)
        {
        }
    }
}
