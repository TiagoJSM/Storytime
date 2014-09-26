using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Entities.SceneWidgets
{
    public enum TranslateArrowDirection
    {
        Vertical,
        Horizontal
    }

    public class TranslateWidgetRenderableAsset : TemplateRenderableAsset
    {
        private ITexture2D _texture;
        private AxisAlignedBoundingBox2D _boundingBox;

        public TranslateWidgetRenderableAsset(IGraphicsContext context, TranslateArrowDirection direction)
        {
            if (direction == TranslateArrowDirection.Horizontal)
            {
                _texture = context.LoadTexture2D("ArrowHorizontal");
                Origin = new Vector2(0, _texture.Height / 2);
            }
            else
            {
                _texture = context.LoadTexture2D("ArrowVertical");
                Origin = new Vector2(_texture.Width / 2, 0);
            }
            _boundingBox = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
        }

        public override void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            Render(renderer, _texture, _boundingBox);
        }

        protected override AxisAlignedBoundingBox2D RawBoundingBox
        {
            get { return _boundingBox; }
        }
    }
}
