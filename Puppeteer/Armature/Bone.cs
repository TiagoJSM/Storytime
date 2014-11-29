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
                    AbsolutePosition = _parent.AbsoluteEnd;
                    Length = lenght;
                }
                SetDirty();
            }
        }

        public Vector2 AbsolutePosition
        {
            get 
            {
                if (Parent == null)
                {
                    return Translation;
                }
                Vector3 translation = new Vector3(Translation.X, Translation.Y, 0.0f);
                Vector3 relativePosition = Vector3.Transform(translation, Parent.RelativeEndTransformation);
                return new Vector2(relativePosition.X, relativePosition.Y);
            }
            set
            {
                if (Parent == null)
                {
                    Translation = value;
                }
                else
                {
                    //TODO: translation should have root from bone end of parent, not from parent origin
                    Vector3 positionVector = new Vector3(value.X, value.Y, 0.0f);
                    Matrix inverted = Matrix.Invert( Parent.RelativeEndTransformation);
                    Vector3 transform = Vector3.Transform(positionVector, inverted);
                    Translation = new Vector2(transform.X, transform.Y);
                }
                SetDirty();
            }
        }
        public Vector2 AbsoluteEnd 
        {
            get
            {
                Vector3 relativeEnd = (LenghtMatrix * Transformation).Translation;
                return new Vector2(relativeEnd.X, relativeEnd.Y);
            }
            set
            {
                Vector2 relativeEnd = AbsoluteEnd;
                if (relativeEnd == value) return;
                Vector2 relativePosition = AbsolutePosition;
                Length = Vector2.Distance(relativePosition, value);
                Rotation = 0;
                float rotationInRelativeEnd = value.AngleWithCenterIn(relativePosition) - 90.0f;
                float actualRotation = AbsoluteEnd.AngleWithCenterIn(relativePosition) - 90.0f;
                Rotation = rotationInRelativeEnd - actualRotation;
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

        public Matrix RelativeEndTransformation
        {
            get { return LenghtMatrix * Transformation; }
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
