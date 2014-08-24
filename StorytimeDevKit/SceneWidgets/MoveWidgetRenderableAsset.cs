using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Input.Time;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.SceneWidgets
{
    public class MoveWidgetRenderableAsset : TemplateRenderableAsset
    {
        private ITexture2D _verticalArrow;
        private ITexture2D _horizontalArrow;

        private Rectanglef _verticalArrowBox;
        private Rectanglef _horizontalArrowBox;

        public Rectanglef VerticalArrowBox { get { return _verticalArrowBox; } }
        public Rectanglef HorizontalArrowBox { get { return _horizontalArrowBox; } }

        public MoveWidgetRenderableAsset(IGraphicsContext context)
        {
            _verticalArrow = context.LoadTexture2D("ArrowVertical");
            _horizontalArrow = context.LoadTexture2D("ArrowHorizontal");
            _verticalArrowBox = 
                new Rectanglef(
                    -_verticalArrow.Width / 2, 
                    0,
                    _verticalArrow.Height,
                    _verticalArrow.Width);

            _horizontalArrowBox =
                new Rectanglef(
                    0,
                    -_horizontalArrow.Height / 2,
                    _horizontalArrow.Height,
                    _horizontalArrow.Width);
            IsVisible = true;
        }

        public override void Render(IRenderer renderer)
        {
            if (!IsVisible) return;
            Render(renderer, _verticalArrow, _verticalArrowBox, 0);
            Render(renderer, _horizontalArrow, _horizontalArrowBox, 0);
        }

        protected override Rectanglef RawBoundingBox
        {
            get
            {
                return _verticalArrowBox.Combine(_horizontalArrowBox);
            }
        }
    }
}
