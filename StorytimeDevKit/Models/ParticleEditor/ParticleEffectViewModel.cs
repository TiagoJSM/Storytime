using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands.ParticleEditor;
using StoryTimeDevKit.Controls;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleEffectViewModel : ParticleTreeViewItem
    {
        private ParticleEffectActor _particleEffectActor;

        public ICommand AddParticleEmitter { get; private set; }

        public ParticleEffectViewModel(
            string name, 
            INodeAddedCallback nodeAddCB, 
            ICommand addParticleEmitterCommand, 
            ParticleEffectActor particleEffectActor)
            : base(name, nodeAddCB, particleEffectActor.ParticleEffectComponent.ParticleEffect)
        {
            AddParticleEmitter = addParticleEmitterCommand;
            _particleEffectActor = particleEffectActor;
        }

    }
}
