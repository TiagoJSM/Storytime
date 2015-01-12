using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.Editors;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEmissorPropertyEditorController
    {
        IParticleEmissorPropertyEditor ParticleEmissorPropertyEditor { get; set; }
    }
}
