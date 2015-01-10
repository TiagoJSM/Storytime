using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.Entities.Renderables
{
    public class FreeMovementRenderableAsset : TemplateRenderableAsset
    {
        private ITexture2D _texture;
        private AxisAlignedBoundingBox2D _boundingBox;

        public FreeMovementRenderableAsset(IGraphicsContext context)
        {
            _texture = context.LoadTexture2D("freeMovementWidget");
            _boundingBox = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
            RenderingOffset = -AABoundingBox.Center;
            Origin = AABoundingBox.Center;
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
