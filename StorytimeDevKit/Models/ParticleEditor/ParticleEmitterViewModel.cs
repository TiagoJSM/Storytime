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
        public ParticleEmitter ParticleEmitter { get; private set; }

        public ParticleSpawnProcessorViewModel ParticleSpawnProcessorViewModel
        {
            get { return Children.OfType<ParticleSpawnProcessorViewModel>().FirstOrDefault(); }
        }

        public ParticleEmitterViewModel(
            string name, 
            INodeAddedCallback nodeAddCB, 
            TreeViewItemViewModel parent,
            ParticleEmitter emitter)
            : base(name, parent, nodeAddCB, emitter)
        {
            ParticleEmitter = emitter;
        }
    }
}
