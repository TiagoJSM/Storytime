using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.General;
using StoryTimeCore.Utils;
using StoryTimeCore.Delegates;
using StoryTimeCore.Entities;

namespace StoryTimeFramework.Entities.Components
{
    public abstract class Component : WorldEntity, IRenderable, IPositionable
    {
        private Vector2 _origin;
        private float _rotation;
        private Vector2 _scale;
        private Vector2 _renderingOffset;

        public event OnRotationChanges OnRotationChanges;
        public event OnPositionChanges OnPositionChanges;

        public IComponentOwner Owner { get; set; }
        public bool RenderInGame { get; protected set; }
        public virtual bool IsVisible { get; set; }
        public override AxisAlignedBoundingBox2D AABoundingBox
        {
            get
            {
                return BoundingBox.GetAABoundingBox();
            }
        }

        public Vector2 Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                if (_origin != value)
                {
                    _origin = value;
                    RaiseBoundingBoxChanges();
                }
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
                if (_rotation != value)
                {
                    _rotation = value;
                    if (OnRotationChanges != null) OnRotationChanges(this);
                    RaiseBoundingBoxChanges();
                }
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
                if (_scale != value)
                {
                    _scale = value;
                    RaiseBoundingBoxChanges();
                }
            }
        }
        public Vector2 RenderingOffset
        {
            get
            {
                return _renderingOffset;
            }
            set
            {
                if (_renderingOffset != value)
                {
                    _renderingOffset = value;
                    if (OnPositionChanges != null) OnPositionChanges(this);
                    RaiseBoundingBoxChanges();
                }
            }
        }

        public override BoundingBox2D BoundingBox
        {
            get 
            {
                if (Owner == null)
                    return RawAABoundingBox.GetBoundingBox2D();

                var box = RawAABoundingBox.GetBoundingBox2D();
                box.Transform(Transformation);
                return box;
            }
        }
        public Matrix Transformation
        {
            get
            {
                var transformation = MatrixUtils.CreateTransformation(this);
                if (Owner == null)
                    return transformation;
                return transformation * Owner.Transformation;
            }
        }
        
        protected abstract AxisAlignedBoundingBox2D RawAABoundingBox { get; }

        public Component()
        {
            IsVisible = true;
            _scale = Vector2.One;
        }

        public void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            DoRender(renderer);
        }

        protected ITexture2D LoadTexture2D(string relativePath)
        {
            return Owner.Scene.GraphicsContext.LoadTexture2D(relativePath);
        }

        protected void RenderTexture(IRenderer renderer, ITexture2D texture)
        {
            renderer.Render(texture, Transformation, RawAABoundingBox);
        }

        protected abstract void DoRender(IRenderer renderer);
    }
}
