using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands.ParticleEditor;
using StoryTimeDevKit.Controllers.ParticleEditor;

namespace StoryTimeDevKit.Models.ParticleEditor
{
    public class ParticleProcessorContextViewModel : BaseViewModel
    {
        private string _name;
        private IParticleEditorActionContext _actionContext;

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

        public ParticleProcessorContextViewModel(Type particleProcessorType, IParticleEditorActionContext actionContext)
        {
            Name = particleProcessorType.Name;
            _actionContext = actionContext;
            AddParticleProcessor = new AddParticleProcessorCommand(actionContext, particleProcessorType);
        }
    }
}
