using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controls.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEditorController : IController<IParticleEditorControl>, IController<IGameObjectsControl>
    {
        IParticleEditorControl ParticleEditorControl { get; set; }

        void AddParticleEmitter();
    }
}
