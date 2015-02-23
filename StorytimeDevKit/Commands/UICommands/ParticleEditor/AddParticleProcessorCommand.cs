using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Commands.UICommands.ParticleEditor
{
    public class AddParticleProcessorCommand : BaseCommand
    {
        private IParticleEditorActionContext _actionContext;
        private Type _particleProcessorType;

        public AddParticleProcessorCommand(IParticleEditorActionContext actionContext, Type particleProcessorType)
        {
            _actionContext = actionContext;
            _particleProcessorType = particleProcessorType;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter is ParticleEmitterViewModel;
        }

        public override void Execute(object parameter)
        {
            var particleEmitter = parameter as ParticleEmitterViewModel;
            _actionContext.AddParticleProcessorTo(particleEmitter, _particleProcessorType);
        }
    }
}
