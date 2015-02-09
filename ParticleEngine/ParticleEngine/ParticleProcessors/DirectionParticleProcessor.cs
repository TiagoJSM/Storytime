using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.CustomAttributes.Editor;
using StoryTimeCore.Utils;

namespace ParticleEngine.ParticleProcessors
{
    public class DirectionParticleProcessor : TemplateParticleProcessor
    {
        [Editable(EditorName = "Initial direction")]
        public Vector2 InitialDirection { get; set; }
        [Editable(EditorName = "Final direction")]
        public Vector2 FinalDirection { get; set; }

        public override void Process(IEnumerable<Particle> particles)
        {
            foreach (var particle in particles)
            {
                particle.Direction =
                    MathematicalUtils
                        .LinearInterpolation(
                            InitialDirection,
                            FinalDirection,
                            particle.TimeToLive,
                            particle.ElapsedLifeTime);
            }
        }
    }
}
