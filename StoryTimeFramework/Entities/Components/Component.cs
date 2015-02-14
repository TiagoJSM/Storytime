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

        public BaseActor OwnerActor { get; set; }
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
                if (OwnerActor == null || OwnerActor.Body == null)
                    return RawAABoundingBox.GetBoundingBox2D();

                var position = OwnerActor.Body.Position;
                var rotation = OwnerActor.Body.Rotation;

                var box = RawAABoundingBox.GetBoundingBox2D();
                box.Translate(position);
                return
                    box
                    .GetRotated(rotation, position);
            }
        }
        public Matrix Transformation
        {
            get
            {
                var transformation = MatrixUtils.CreateTransformation(this);
                if (OwnerActor.Body == null)
                    return transformation;
                var body = OwnerActor.Body;
                var localTransform =
                    MatrixUtils.CreateLocalTransformation(
                        body.Position,
                        body.Rotation,
                        Vector2.One);
                return transformation * localTransform;
            }
        }
        
        protected abstract AxisAlignedBoundingBox2D RawAABoundingBox { get; }

        public Component()
        {
            _scale = Vector2.One;
        }

        public void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            DoRender(renderer);
        }

        protected ITexture2D LoadTexture2D(string relativePath)
        {
            return OwnerActor.Scene.GraphicsContext.LoadTexture2D(relativePath);
        }

        protected void RenderTexture(IRenderer renderer, ITexture2D texture)
        {
            renderer.Render(texture, Transformation, RawAABoundingBox);
        }

        protected abstract void DoRender(IRenderer renderer);
    }
}
