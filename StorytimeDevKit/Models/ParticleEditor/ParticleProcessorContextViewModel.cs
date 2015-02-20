using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleProcessorContextViewModel : BaseViewModel
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
        public ICommand AddParticleProcessor { get; private set; }

        public ParticleProcessorContextViewModel(Type particleProcessorType, ICommand addParticleProcessor)
        {
            Name = particleProcessorType.Name;
            AddParticleProcessor = addParticleProcessor;
        }
    }
}
