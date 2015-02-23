using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands.ParticleEditor;
using StoryTimeDevKit.Controllers.ParticleEditor;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleSpawnProcessorContextViewModel : BaseViewModel
    {
        private string _name;
        private Type _particleSpawnProcessorType;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public ICommand ReplaceParticleSpawnProcessor { get; private set; }

        public ParticleSpawnProcessorContextViewModel(Type particleSpawnProcessorType, IParticleEditorActionContext actionContext)
        {
            _particleSpawnProcessorType = particleSpawnProcessorType;
            ReplaceParticleSpawnProcessor = new ReplaceParticleSpawnProcessorCommand(particleSpawnProcessorType,
                actionContext);
            Name = particleSpawnProcessorType.Name;
        }
    }
}
