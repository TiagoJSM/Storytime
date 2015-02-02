using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controls.ParticleEditor
{
    public interface IParticleEffectTreeView
    {
        event Action<ParticleTreeViewItem> OnSelectedItemChanged;
    }
}
