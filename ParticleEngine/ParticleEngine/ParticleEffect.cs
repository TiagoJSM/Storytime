using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Physics;

namespace ParticleEngine
{
    public class ParticleEffect
    {
        private IParticleBodyFactory _particleBodyFactory;
        private List<ParticleEmitter> _particleEmitters;
        private Vector2 _position;

        public IEnumerable<ParticleEmitter> ParticleEmitters { get { return _particleEmitters; } }

        [Editable]
        public string Name { get; set; }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                var difference = value - _position;
                _position = value;
                foreach (var emitter in _particleEmitters)
                    emitter.Position = emitter.Position + difference;
            }
        }

        public event Action<ParticleEmitter> OnParticleEmitterSpawned;

        public ParticleEffect(IParticleBodyFactory particleBodyFactory)
        {
            _particleEmitters = new List<ParticleEmitter>();
            _particleBodyFactory = particleBodyFactory;
        }

        public ParticleEmitter AddEmitter()
        {
            var particleEmitter = new ParticleEmitter(_particleBodyFactory);
            _particleEmitters.Add(particleEmitter);
            if (OnParticleEmitterSpawned != null)
                OnParticleEmitterSpawned(particleEmitter);
            return particleEmitter;
        }

        public void TimeElapse(TimeSpan elapsedSinceLastUpdate)
        {
            foreach(var emitter in _particleEmitters.ToArray())
            {
                emitter.TimeElapse(elapsedSinceLastUpdate);
            }
        }
    }
}
