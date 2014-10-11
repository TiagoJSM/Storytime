using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Physics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace FarseerPhysicsWrapper
{
    public class FarseerBody : IBody
    {
        private Body _body;
        private Vector2 _position;
        private float _rotation;
        private Vector2 _scale;

        public event Action<IBody> OnPositionChanges;
        public event Action<IBody> OnRotationChanges;
        public event Action<IBody> OnScaleChanges;

        public Vector2 Position 
        {
            get
            {
                return _position;
            }
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
            get
            {
                return _rotation;
            }
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                if (OnRotationChanges != null)
                    OnRotationChanges(this);
            }
        }

        public Vector2 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                if (_scale == value) return;
                _scale = value;
                if (OnScaleChanges != null)
                    OnScaleChanges(this);
            }
        }

        public FarseerBody(Body body)
        {
            _body = body;
            _scale = new Vector2(1);
        }

        public void Synchronize()
        {
            Position = _body.Position;
        }
    }
}
