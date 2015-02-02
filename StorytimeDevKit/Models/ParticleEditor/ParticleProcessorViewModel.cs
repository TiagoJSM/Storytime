using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ParticleEngine.ParticleProcessors;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleProcessorViewModel : ParticleTreeViewItem
    {
        public ICommand RemoveParticleProcessor { get; private set; }
        public IParticleProcessor ParticleProcessor { get; private set; }

        public ParticleProcessorViewModel(IParticleProcessor particleProcessor, TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB,
            ICommand removeParticleProcessor)
            : base(particleProcessor.GetType().Name, parent, nodeAddCB, particleProcessor)
        {
            RemoveParticleProcessor = removeParticleProcessor;
            ParticleProcessor = particleProcessor;
        }
    }
}
