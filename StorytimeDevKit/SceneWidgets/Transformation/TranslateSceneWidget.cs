using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using Microsoft.Xna.Framework;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.SceneWidgets;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public class TranslateSceneWidget : BaseWidget
    {
        private class ArrowTranslateSceneWidget : BaseWidget
        {
            private TranslateWidgetRenderableAsset _asset;
            private TranslateArrowDirection _direction;

            public ArrowTranslateSceneWidget(TranslateArrowDirection direction)
            {
                OnInitialize += OnInitializeHandler;
                _direction = direction;
            }

            protected override IRenderableAsset RenderableAsset
            {
                get { return _asset; }
            }

            public override bool Intersects(Vector2 point)
            {
                return _asset.BoundingBox.Contains(point);
            }

            private void OnInitializeHandler()
            {
                _asset = new TranslateWidgetRenderableAsset(GraphicsContext, _direction);
            }
        }

        public TranslateSceneWidget()
        {
            OnInitialize += OnInitializeHandler;
        }

        public override bool Intersects(Vector2 point)
        {
            foreach (BaseWidget child in Children)
                if (child.Intersects(point))
                    return true;
            return false;
        }

        private void OnInitializeHandler()
        {
            Children.Add(new ArrowTranslateSceneWidget(TranslateArrowDirection.Horizontal));
            Children.Add(new ArrowTranslateSceneWidget(TranslateArrowDirection.Vertical));
        }
    }
}
