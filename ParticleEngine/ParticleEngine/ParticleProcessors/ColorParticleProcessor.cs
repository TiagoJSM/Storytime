using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Utils;

namespace ParticleEngine.ParticleProcessors
{
    public class ColorParticleProcessor : TemplateParticleProcessor
    {
        [Editable(EditorName = "Initial color")]
        public Color InitialColor { get; set; }
        [Editable(EditorName = "Final color")]
        public Color FinalColor { get; set; }

        public override void Process(IEnumerable<Particle> particles)
        {
            foreach (var particle in particles)
            {
                particle.Color =
                    MathematicalUtils
                        .LinearInterpolation(
                            InitialColor,
                            FinalColor,
                            particle.TimeToLive,
                            particle.ElapsedLifeTime);
            }
        }
    }
}
