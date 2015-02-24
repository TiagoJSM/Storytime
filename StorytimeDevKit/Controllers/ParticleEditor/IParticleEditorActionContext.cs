using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEngine;
using StoryTimeDevKit.Models.ParticleEditor;

namespace StoryTimeDevKit.Controllers.ParticleEditor
{
    public interface IParticleEditorActionContext
    {
        void AddParticleEmitterTo(ParticleEffectViewModel particleEffect);
        void SetParticleSpawnProcessorTo(ParticleEmitterViewModel particleEmitter);
        void AddParticleProcessorTo(ParticleEmitterViewModel particleEmitter, Type particleProcessorType);
        void RemoveParticleProcessorFromEmitter(ParticleProcessorViewModel particleProcessor, ParticleEmitter particleEmitter);
        void ReplaceParticleSpawnProcessorFromEmitter(ParticleEmitterViewModel particleEmitter, Type spawnProcessorType);
    }
}
