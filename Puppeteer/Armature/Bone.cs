using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Puppeteer.Armature
{
    public class Bone
    {
        private List<Bone> _children;
        private Bone _parent;
        private bool _transformationMatrixIsDirty;
        private Matrix _transformation;
        private Vector2 _translation;
        private float _rotation;

        public Bone Parent { get { return _parent; } }
        public Vector2 RelativeOrigin { get; set; }
        public Vector2 RelativeEnd { get; set; }
        public float Length
        {
            get
            {
                return Vector2.Distance(RelativeOrigin, RelativeEnd);
            }
        }
        public List<Bone> Children
        {
            get
            {
                return _children;
            }
        }
        public Vector2 Translation 
        {
            get
            {
                return _translation;
            }
            set
            {
                if (_translation == value)
                    return;
                _translation = value;
                _transformationMatrixIsDirty = true;
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
                if (_rotation == value)
                    return;
                _rotation = value;
                _transformationMatrixIsDirty = true;
            }
        }
        public Matrix Transformation
        {
            get 
            {
                if (!_transformationMatrixIsDirty)
                    return _transformation;

                Matrix rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation));
                Matrix translation = Matrix.CreateTranslation(Translation.X, Translation.Y, 0);
                _transformation = rotation * translation;
                if(_parent != null)
                    _transformation = Parent.Transformation * _transformation;
                _transformationMatrixIsDirty = false;
                return _transformation;
            }
        }

        public Bone()
        {
            _children = new List<Bone>();
            _transformationMatrixIsDirty = true;
        }

        public Bone(Bone parent)
            :this()
        {
            _parent = parent;
        }
    }
}
