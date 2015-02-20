using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ParticleEngine;

namespace StoryTimeFramework.Entities.Components
{
    public class ParticleComponent : Static2DComponent
    {
        private Particle _particle;
        public Particle Particle
        {
            get { return _particle; }
            set
            {
                _particle = value;
                Texture2DName = _particle.ParticleTexture;
            }
        }

        public ParticleComponent()
        {
            OnCreated += OnCreatedHandler;
        }

        private void OnCreatedHandler()
        {
            Origin = new Vector2(RawAABoundingBox.Width / 2, 0.0f);
            RenderingOffset = -new Vector2(RawAABoundingBox.Width / 2, 0.0f);
        }
    }
}
