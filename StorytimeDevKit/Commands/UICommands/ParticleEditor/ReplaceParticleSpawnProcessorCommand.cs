using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine.ParticleProcessors.ParticleSpawnProcessors;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Commands.UICommands.ParticleEditor
{
	public class ReplaceParticleSpawnProcessorCommand : BaseCommand
	{
		private IParticleEditorActionContext _actionContext;

		public ReplaceParticleSpawnProcessorCommand(IParticleEditorActionContext actionContext)
		{
			_actionContext = actionContext;
		}

		public override bool CanExecute(object parameter)
		{
			return parameter is ParticleSpawnProcessorViewModel;
		}

		public override void Execute(object parameter)
		{
            var particleProcessor = parameter as ParticleSpawnProcessorViewModel;
			var particleEmitter = (particleProcessor.Parent as ParticleEmitterViewModel).ParticleEmitter;
			_actionContext.ReplaceParticleSpawnProcessorFromEmitter(particleEmitter, typeof(DefaultParticleSpawnProcessor));
		}
	}
}
