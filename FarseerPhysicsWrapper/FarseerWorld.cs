using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Physics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace FarseerPhysicsWrapper
{
    public class FarseerPhysicalWorld : IPhysicalWorld
    {
        private World _world;

        public Vector2 Gravity
        {
            get { return _world.Gravity; }
            set { _world.Gravity = value; }
        }

        public FarseerPhysicalWorld(Vector2 gravity)
        {
            _world = new World(gravity);
        }

        public IBody CreateRectangularBody(float width, float height, float density, object userData = null)
        {
            return new FarseerBody(BodyFactory.CreateRectangle(_world, width, height, density, userData));
        }

        public IBody CreateParticleBody(bool physicalSimulated, float width, float height, float density)
        {
            if (physicalSimulated)
                return new FarseerBody(BodyFactory.CreateRectangle(_world, width, height, density));
            throw new NotImplementedException();
        }
    }
}
