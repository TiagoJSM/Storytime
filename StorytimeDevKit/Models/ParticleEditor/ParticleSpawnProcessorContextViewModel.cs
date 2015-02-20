using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleSpawnProcessorContextViewModel : BaseViewModel
    {
        private string _name;

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

        public ParticleSpawnProcessorContextViewModel(Type particleProcessorType, ICommand replaceParticleSpawnProcessor)
        {
            Name = particleProcessorType.Name;
            ReplaceParticleSpawnProcessor = replaceParticleSpawnProcessor;
        }
    }
}
