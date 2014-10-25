using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

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
        private float _lenght;

        public Bone Parent 
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent == value) return;

                if (_parent != null)
                {
                    _parent._children.Remove(this);
                }
                _parent = value;
                if (_parent != null)
                {
                    _parent._children.Add(this);
                    float lenght = Length;
                    RelativePosition = _parent.RelativeEnd;
                    Length = lenght;
                }
                SetDirty();
            }
        }

        public Vector2 RelativePosition
        {
            get 
            {
                Vector3 translation = Transformation.Translation;
                return new Vector2(translation.X, translation.Y);
            }
            set
            {
                if (Parent == null)
                {
                    Translation = value;
                }
                else
                {
                    Translation = value - Parent.RelativeEnd;
                }
                SetDirty();
            }
        }
        public Vector2 RelativeEnd 
        {
            get
            {
                Vector3 relativeEnd = (LenghtMatrix * Transformation).Translation;
                return new Vector2(relativeEnd.X, relativeEnd.Y);
            }
            set
            {
                Vector2 relativeEnd = RelativeEnd;
                if (relativeEnd == value) return;
                Vector2 relativePosition = RelativePosition;
                Length = Vector2.Distance(relativePosition, value);
                Rotation = value.AngleWithCenterIn(relativePosition) - 90.0f;
            }
        }
        public float Length
        {
            get
            {
                return _lenght;
            }
            set
            {
                if (_lenght == value) return;
                _lenght = value;
                SetDirty();
            }
        }
        public IEnumerable<Bone> Children
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
                SetDirty();
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
                SetDirty();
            }
        }
        public Matrix Transformation
        {
            get 
            {
                if (!_transformationMatrixIsDirty)
                    return _transformation;

                Matrix rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation));
                float parentLength = 0;
                if(Parent != null)
                {
                    parentLength = Parent.Length;
                }
                
                Matrix translation = 
                    Matrix
                        .CreateTranslation(Translation.X, Translation.Y + parentLength, 0);
                _transformation = rotation * translation;
                if (Parent != null)
                    _transformation = _transformation * Parent.Transformation;
                _transformationMatrixIsDirty = false;
                return _transformation;
            }
        }

        private Matrix LenghtMatrix
        {
            get
            {
                return Matrix.CreateTranslation(0, _lenght, 0);
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
            Parent = parent;
        }

        public void AddChildren(Bone children)
        {
            if (_children.Contains(children)) return;
            children.Parent = this;
        }

        public void RemoveChildren(Bone children)
        {
            if (!_children.Contains(children)) return;
            children.Parent = null;
        }

        public void SetDirty()
        {
            _transformationMatrixIsDirty = true;
            foreach (Bone child in _children)
                child.SetDirty();
        }


    }
}
