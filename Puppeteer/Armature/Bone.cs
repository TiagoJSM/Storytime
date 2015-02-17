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
                    var lenght = Length;
                    AbsolutePosition = _parent.AbsoluteEnd;
                    Length = lenght;
                }
                SetDirty();
            }
        }

        public string Name { get; set; }
        public Vector2 AbsolutePosition
        {
            get 
            {
                if (Parent == null)
                {
                    return Translation;
                }
                var translation = new Vector3(Translation.X, Translation.Y, 0.0f);
                var relativePosition = Vector3.Transform(translation, Parent.RelativeEndTransformation);
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
                    var positionVector = new Vector3(value.X, value.Y, 0.0f);
                    var inverted = Matrix.Invert( Parent.RelativeEndTransformation);
                    var transform = Vector3.Transform(positionVector, inverted);
                    Translation = new Vector2(transform.X, transform.Y);
                }
                SetDirty();
            }
        }
        public Vector2 AbsoluteEnd 
        {
            get
            {
                var relativeEnd = (LenghtMatrix * Transformation).Translation;
                return new Vector2(relativeEnd.X, relativeEnd.Y);
            }
            set
            {
                var relativeEnd = AbsoluteEnd;
                if (relativeEnd == value) return;
                var relativePosition = AbsolutePosition;
                Length = Vector2.Distance(relativePosition, value);
                Rotation = 0;
                var rotationInRelativeEnd = value.AngleWithCenterIn(relativePosition) - 90.0f;
                var actualRotation = AbsoluteEnd.AngleWithCenterIn(relativePosition) - 90.0f;
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

                var rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation));
                float parentLength = 0;
                if(Parent != null)
                {
                    parentLength = Parent.Length;
                }
                
                var translation = 
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

        public Bone(string name = null)
        {
            _children = new List<Bone>();
            _transformationMatrixIsDirty = true;
            Name = name;
        }

        public Bone(Bone parent, string name = null)
            : this(name)
        {
            Parent = parent;
            if (parent != null)
             Parent.AddChildren(this);
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
            foreach (var child in _children)
                child.SetDirty();
        }
    }
}
