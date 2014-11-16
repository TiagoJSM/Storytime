using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Input.Time;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Delegates;

namespace StoryTimeFramework.Resources.Graphic
{
    public abstract class TemplateRenderableAsset : IRenderableAsset
    {
        private Vector2 _origin;
        private float _rotation;
        private Vector2 _scale;
        private Vector2 _renderingOffset;

        public event OnBoundingBoxChanges OnBoundingBoxChanges;
        public event OnRotationChanges OnRotationChanges;
        public event OnPositionChanges OnPositionChanges;

        public virtual bool IsVisible { get; set; }

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
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
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
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
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
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
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
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
                }
            }
        }

        public TemplateRenderableAsset()
        {
            IsVisible = true;
            _scale = new Vector2(1);
        }

        protected void Render(IRenderer renderer, ITexture2D texture, AxisAlignedBoundingBox2D boundingBox)
        {
            if (!IsVisible) return;

            renderer
                .Render(
                    texture,
                    boundingBox.X,
                    boundingBox.Y,
                    boundingBox.Width,
                    boundingBox.Height,
                    Rotation,
                    _origin,
                    _renderingOffset
                );
        }

        public abstract void Render(IRenderer renderer);
        public virtual void TimeElapse(WorldTime WTime) { }
        public AxisAlignedBoundingBox2D AABoundingBox 
        {
            get
            {
                AxisAlignedBoundingBox2D computedBox = RawBoundingBox;
                computedBox.Translate(RenderingOffset);
                return computedBox.GetScaled(_scale, Vector2.Zero).GetRotated(_rotation);
            }
        }
        public BoundingBox2D BoundingBox
        {
            get 
            {
                BoundingBox2D computedBox = RawBoundingBox.GetBoundingBox2D();
                computedBox.Translate(RenderingOffset);
                return computedBox.GetScaled(_scale, Vector2.Zero).GetRotated(_rotation);
            }
        }
        public AxisAlignedBoundingBox2D BoundingBoxWithoutOrigin
        {
            get
            {
                AxisAlignedBoundingBox2D computedBox = RawBoundingBox;
                return computedBox.GetScaled(_scale, Vector2.Zero).GetRotated(_rotation);
            }
        }

        protected void RaiseOnBoundingBoxChanges()
        {
            if(OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
        }

        protected abstract AxisAlignedBoundingBox2D RawBoundingBox { get; }
    }
}
