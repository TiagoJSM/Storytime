using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Commands.UICommands.ParticleEditor
{
    public class AddParticleEmitterCommand : BaseCommand
    {
        private IParticleEditorActionContext _actionContext;

        public AddParticleEmitterCommand(IParticleEditorActionContext actionContext)
        {
            _actionContext = actionContext;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter is ParticleEffectViewModel;
        }

        public override void Execute(object parameter)
        {
            var particleEffect = parameter as ParticleEffectViewModel;
            _actionContext.AddParticleEmitterTo(particleEffect);
        }
    }
}
