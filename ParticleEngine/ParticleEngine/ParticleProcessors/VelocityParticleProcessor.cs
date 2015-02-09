using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Manageables;
using StoryTimeCore.Utils;

namespace ParticleEngine.ParticleProcessors
{
    public class VelocityParticleProcessor : TemplateParticleProcessor
    {
        [Editable(EditorName = "Initial velocity")]
        public float InitialVelocity { get; set; }
        [Editable(EditorName = "Final velocity")]
        public float FinalVelocity { get; set; }

        public override void Process(IEnumerable<Particle> particles)
        {
            foreach (var particle in particles)
            {
                particle.Velocity =
                    MathematicalUtils
                        .LinearInterpolation(
                            InitialVelocity,
                            FinalVelocity,
                            particle.TimeToLive,
                            particle.ElapsedLifeTime);
            }
        }
    }
}
