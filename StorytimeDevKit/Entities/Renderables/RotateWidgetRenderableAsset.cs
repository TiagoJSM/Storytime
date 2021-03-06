﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Entities.Renderables
{
    public class RotateWidgetRenderableAsset : TemplateRenderableAsset
    {
        private ITexture2D _texture;
        private AxisAlignedBoundingBox2D _boundingBox;

        public RotateWidgetRenderableAsset(IGraphicsContext context)
        {
            _texture = context.LoadTexture2D("RotationWidget");
            _boundingBox = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
            RenderingOffset = - _boundingBox.Center;
            Origin = _boundingBox.Center;
        }

        public override void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            Render(renderer, _texture, _boundingBox);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return _boundingBox; }
        }
    }
}
