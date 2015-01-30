using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Controls.ParticleEditor;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleEmitterViewModel : ParticleTreeViewItem
    {
        public ICommand SetParticleSpawnProcessor { get; private set; }
        public ICommand SetParticleProcessor { get; private set; }

        public ParticleEmitterViewModel(
            string name, 
            INodeAddedCallback nodeAddCB, 
            ICommand setParticleSpawnProcessor, 
            ICommand setParticleProcessor,
            TreeViewItemViewModel parent)
            : base(name, parent, nodeAddCB)
        {
            SetParticleSpawnProcessor = setParticleSpawnProcessor;
            SetParticleProcessor = setParticleProcessor;
        }
    }
}
