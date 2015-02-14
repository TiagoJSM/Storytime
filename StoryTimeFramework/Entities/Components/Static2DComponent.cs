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
    public class Static2DComponent : Component
    {
        private Static2DRenderableAsset _renderableAsset;
        private ITexture2D _texture;

        public string Texture2DName { get; set; }

        public AxisAlignedBoundingBox2D AABoundingBoxWithoutOrigin
        {
            get { return BoundingBoxWithoutOrigin.GetAABoundingBox(); }
        }

        public BoundingBox2D BoundingBoxWithoutOrigin
        {
            get
            {
                var boundingBox = RawAABoundingBox.GetBoundingBox2D();
                boundingBox.Transform(Transformation);
                return boundingBox;
            }
        }

        public Static2DComponent()
        {
            IsVisible = true;
            OnCreated += OnCreatedHandler;
        }

        protected override void DoRender(IRenderer renderer)
        {
            RenderTexture(renderer, _texture);
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return _texture.GetAABoundingBox(); }
        }

        private void OnCreatedHandler()
        {
            _texture = LoadTexture2D(Texture2DName);
        }
    }
}
