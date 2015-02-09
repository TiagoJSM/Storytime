using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Commands.UICommands.ParticleEditor
{
    public class RemoveParticleProcessorCommand : BaseCommand
    {
         private IParticleEditorActionContext _actionContext;

         public RemoveParticleProcessorCommand(IParticleEditorActionContext actionContext)
        {
            _actionContext = actionContext;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter is ParticleProcessorViewModel;
        }

        public override void Execute(object parameter)
        {
            var particleProcessor = parameter as ParticleProcessorViewModel;
            var particleEmitter = (particleProcessor.Parent as ParticleEmitterViewModel).ParticleEmitter;
            _actionContext.RemoveParticleProcessorFromEmitter(particleProcessor, particleEmitter);
        }
    }
}
