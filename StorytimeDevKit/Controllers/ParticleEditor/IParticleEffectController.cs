﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.ParticleEditor;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEffectController
    {
        ObservableCollection<ParticleEffectViewModel> ParticleEffectViewModels { get; }
        IParticleEffectTreeView ParticleEffectControl { get; set; }
        ObservableCollection<ParticleProcessorContextViewModel> ParticleProcessors { get; }
        ObservableCollection<ParticleSpawnProcessorContextViewModel> ParticleSpawnProcessors { get; }
    }
}
