using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Delegates;
using StoryTimeCore.Extensions;
using StoryTimeCore.General;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Utils;
using StoryTimeFramework.Resources.Graphic;

namespace StoryTimeFramework.Entities.Components
{
    public class Static2DComponent : Component, IPositionable
    {
        private Vector2 _origin;
        private float _rotation;
        private Vector2 _scale;
        private Vector2 _renderingOffset;
        private Static2DRenderableAsset _renderableAsset;

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

        public string Texture2DName { get; set; }

        public AxisAlignedBoundingBox2D AABoundingBoxWithoutOrigin
        {
            get { return BoundingBoxWithoutOrigin.GetAABoundingBox(); }
        }

        public BoundingBox2D BoundingBoxWithoutOrigin
        {
            get
            {
                var boundingBox = _renderableAsset.BoundingBox;
                boundingBox.Transform(Transformation);
                return boundingBox;
            }
        }

        protected virtual Matrix Transformation
        {
            get
            {
                return MatrixUtils.CreateRenderableAssetTransformation(this);
            }
        }

        public Static2DComponent()
        {
            OnCreated += OnCreatedHandler;
        }

        public override void Render(IRenderer renderer)
        {
            
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return _renderableAsset.AABoundingBox; }
        }

        private void OnCreatedHandler()
        {
            var texture = LoadTexture2D(Texture2DName);
            _renderableAsset = new Static2DRenderableAsset()
            {
                Texture2D = texture
            };
        }
    }
}
