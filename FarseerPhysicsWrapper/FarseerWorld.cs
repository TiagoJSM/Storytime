using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysicsWrapper.Bodies;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Physics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace FarseerPhysicsWrapper
{
    public class FarseerPhysicalWorld : IPhysicalWorld
    {
        private World _world;
        private List<FakePhysicsSimulationBody> _fakePhysicsSimulationBodies;

        public Vector2 Gravity
        {
            get { return _world.Gravity; }
            set { _world.Gravity = value; }
        }

        public FarseerPhysicalWorld(Vector2 gravity)
        {
            _world = new World(gravity);
            _fakePhysicsSimulationBodies = new List<FakePhysicsSimulationBody>();
        }

        public IBody CreateRectangularBody(float width, float height, float density, object userData = null)
        {
            return new FarseerBody(BodyFactory.CreateRectangle(_world, width, height, density, userData));
        }

        public void Update(WorldTime WTime)
        {
            var totalSeconds = (float) WTime.ElapsedSinceLastTime.TotalSeconds;
            //_world.Step(totalSeconds);
            foreach (var body in _fakePhysicsSimulationBodies)
            {
                body.Position = body.Position + (body.Direction*body.Velocity*totalSeconds);
            }
        }

        public IBody CreateParticleBody(bool physicalSimulated, float width, float height, float density)
        {
            if (physicalSimulated)
                return new FarseerBody(BodyFactory.CreateRectangle(_world, width, height, density));
            var body = new FakePhysicsSimulationBody();
            _fakePhysicsSimulationBodies.Add(body);
            return body;
        }
    }
}
