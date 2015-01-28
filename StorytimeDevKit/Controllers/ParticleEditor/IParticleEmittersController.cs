using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEmittersController
    {
        ObservableCollection<ParticleTreeViewItem> ParticleTreeViewItems { get; }
    }
}
