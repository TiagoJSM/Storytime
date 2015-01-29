using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEffectController
    {
        ObservableCollection<ParticleEffectViewModel> ParticleEffectViewModel { get; }
    }
}
