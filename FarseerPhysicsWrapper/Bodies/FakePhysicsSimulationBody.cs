using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Physics;

namespace FarseerPhysicsWrapper.Bodies
{
    public class FakePhysicsSimulationBody : IBody
    {
        private Vector2 _position;
        private float _rotation;
        private Vector2 _direction;

        public event Action<IBody> OnPositionChanges;
        public event Action<IBody> OnRotationChanges;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                _position = value;
                if (OnPositionChanges != null)
                    OnPositionChanges(this);
            }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                if (OnRotationChanges != null)
                    OnRotationChanges(this);
            }
        }

        public Vector2 Direction
        {
            get { return _direction; }
            set 
            { 
                value.Normalize(); 
                _direction = value; 
            }
        }
        public float Velocity { get; set; }

        public void Synchronize()
        {
            throw new NotImplementedException();
        }
    }
}
