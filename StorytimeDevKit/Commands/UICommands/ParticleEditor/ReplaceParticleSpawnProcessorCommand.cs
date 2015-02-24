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
	    private Type _spawnProcessorType;

		public ReplaceParticleSpawnProcessorCommand(Type spawnProcessorType, IParticleEditorActionContext actionContext)
		{
			_actionContext = actionContext;
		    _spawnProcessorType = spawnProcessorType;
		}

		public override bool CanExecute(object parameter)
		{
			return parameter is ParticleSpawnProcessorViewModel;
		}

		public override void Execute(object parameter)
		{
            var particleProcessor = parameter as ParticleSpawnProcessorViewModel;
			var particleEmitter = particleProcessor.Parent as ParticleEmitterViewModel;
            _actionContext.ReplaceParticleSpawnProcessorFromEmitter(particleEmitter, _spawnProcessorType);
		}
	}
}
