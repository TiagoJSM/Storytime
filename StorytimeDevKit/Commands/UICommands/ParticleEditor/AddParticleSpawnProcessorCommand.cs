using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Commands.UICommands.ParticleEditor
{
    public class AddParticleSpawnProcessorCommand : BaseCommand
    {
        private IParticleEditorActionContext _actionContext;

        public AddParticleSpawnProcessorCommand(IParticleEditorActionContext actionContext)
        {
            _actionContext = actionContext;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter is ParticleEmitterViewModel;
        }

        public override void Execute(object parameter)
        {
            var particleEmitter = parameter as ParticleEmitterViewModel;
            _actionContext.SetParticleSpawnProcessorTo(particleEmitter);
        }
    }
}
