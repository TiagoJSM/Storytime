using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ParticleEngine.ParticleProcessors.ParticleSpawnProcessors;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleSpawnProcessorViewModel : ParticleTreeViewItem
    {
        public ICommand ReplaceParticleSpawnProcessor { get; private set; }
        public ParticleSpawnProcessor SpawnProcessor { get; private set; }

        public ParticleSpawnProcessorViewModel(ParticleSpawnProcessor spawnProcessor, TreeViewItemViewModel parent, INodeAddedCallback nodeAddCB,
            ICommand replaceParticleSpawnProcessor)
            : base(spawnProcessor.GetType().Name, parent, nodeAddCB, spawnProcessor)
        {
            ReplaceParticleSpawnProcessor = replaceParticleSpawnProcessor;
            SpawnProcessor = spawnProcessor;
        }
    }
}
