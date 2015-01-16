using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Physics;

namespace ParticleEngine
{
    public class Particle
    {
        private TimeSpan _timeToLive;

        public IBody Body { get; private set; }
        public Color Color { get; set; }
        public Vector2 Direction { get; set; }
        public double Velocity { get; set; }
        public Vector2 Size { get; set; }

        public Vector2 Position
        {
            get { return Body.Position; }
            set { Body.Position = value; }
        }

        public bool IsAlive
        {
            get
            {
                if (ElapsedLifeTime.Ticks >= TimeToLive.Ticks)
                    return false;
                return true;
            }
        }

        public TimeSpan TimeToLive { get; set; }
        public TimeSpan ElapsedLifeTime { get; set; }

        public Particle(IBody body)
        {
            Body = body;
            Color = new Color(1f, 1f, 1f);
        }
    }
}
