using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Input.Time;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;
using StoryTimeFramework.Resources.Graphic;

namespace StoryTimeDevKit.Entities.Renderables
{
    public class MoveWidgetRenderableAsset : TemplateRenderableAsset
    {
        private ITexture2D _verticalArrow;
        private ITexture2D _horizontalArrow;

        private AxisAlignedBoundingBox2D _verticalArrowBox;
        private AxisAlignedBoundingBox2D _horizontalArrowBox;

        public AxisAlignedBoundingBox2D VerticalArrowBox { get { return _verticalArrowBox; } }
        public AxisAlignedBoundingBox2D HorizontalArrowBox { get { return _horizontalArrowBox; } }

        public MoveWidgetRenderableAsset(IGraphicsContext context)
        {
            _verticalArrow = context.LoadTexture2D("ArrowVertical");
            _horizontalArrow = context.LoadTexture2D("ArrowHorizontal");
            _verticalArrowBox = 
                new AxisAlignedBoundingBox2D(
                    -_verticalArrow.Width / 2, 
                    0,
                    _verticalArrow.Height,
                    _verticalArrow.Width);

            _horizontalArrowBox =
                new AxisAlignedBoundingBox2D(
                    0,
                    -_horizontalArrow.Height / 2,
                    _horizontalArrow.Height,
                    _horizontalArrow.Width);
        }

        public override void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            Render(renderer, _verticalArrow, _verticalArrowBox);
            Render(renderer, _horizontalArrow, _horizontalArrowBox);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get
            {
                return _verticalArrowBox.Combine(_horizontalArrowBox);
            }
        }
    }
}
