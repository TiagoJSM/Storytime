using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEditorActionContext
    {
        void AddParticleEmitterTo(ParticleEffectViewModel particleEffect);
        void SetParticleSpawnProcessorTo(ParticleEmitterViewModel particleEmitter);
        void AddParticleProcessorTo(ParticleEmitterViewModel particleEmitter);
    }
}
