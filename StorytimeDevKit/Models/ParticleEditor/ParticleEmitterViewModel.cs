using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ParticleEngine;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Controls.ParticleEditor;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleEmitterViewModel : ParticleTreeViewItem
    {
        public ICommand SetParticleSpawnProcessor { get; private set; }
        public ICommand AddParticleProcessor { get; private set; }
        public ParticleEmitter ParticleEmitter { get; private set; }

        public ParticleEmitterViewModel(
            string name, 
            INodeAddedCallback nodeAddCB, 
            ICommand setParticleSpawnProcessor, 
            ICommand addParticleProcessor,
            TreeViewItemViewModel parent,
            ParticleEmitter emitter)
            : base(name, parent, nodeAddCB, emitter)
        {
            SetParticleSpawnProcessor = setParticleSpawnProcessor;
            AddParticleProcessor = addParticleProcessor;
            ParticleEmitter = emitter;
        }
    }
}
