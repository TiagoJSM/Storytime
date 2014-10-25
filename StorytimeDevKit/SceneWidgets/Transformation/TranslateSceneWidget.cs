using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using Microsoft.Xna.Framework;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.SceneWidgets;
using StoryTimeCore.DataStructures;
using StoryTimeUI.Delegates;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public delegate void OnTranslate(Vector2 translation);
    public delegate void OnTranslationComplete(Vector2 fromPosition, Vector2 toPosition);

    public class TranslateSceneWidget : BaseWidget
    {
        private class ArrowTranslateSceneWidget : BaseWidget
        {
            private TranslateWidgetRenderableAsset _asset;
            private TranslateArrowDirection _direction;
            private TranslateSceneWidget _translateWidget;

            public ArrowTranslateSceneWidget(TranslateSceneWidget translateWidget, TranslateArrowDirection direction)
            {
                OnInitialize += OnInitializeHandler;
                _direction = direction;
                _translateWidget = translateWidget;
                OnStartDrag += OnStartDragHandler;
                OnDrag += OnDragHandler;
                OnStopDrag += OnStopDragHandler;
            }

            protected override IRenderableAsset RenderableAsset
            {
                get { return _asset; }
            }

            public override bool Intersects(Vector2 point)
            {
                AxisAlignedBoundingBox2D bounds = _asset.AABoundingBox;
                if (Parent != null)
                    bounds.Translate(Parent.Position);
                return bounds.Contains(point);
            }

            private void OnInitializeHandler()
            {
                _asset = new TranslateWidgetRenderableAsset(GraphicsContext, _direction);
            }

            private void OnStartDragHandler(Vector2 currentPosition)
            {
                _translateWidget._startDragPositon = _translateWidget.Position;
            }

            private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
            {
                if(_direction == TranslateArrowDirection.Horizontal)
                    _translateWidget.Translate(new Vector2(dragged.X, 0.0f));
                else
                    _translateWidget.Translate(new Vector2(0.0f, dragged.Y));
            }

            private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
            {
                _translateWidget.TranslationComplete();
            }
        }

        private Vector2 _startDragPositon;

        public event OnTranslate OnTranslate;
        public event OnTranslationComplete OnTranslationComplete;

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
            Children.Add(new ArrowTranslateSceneWidget(this, TranslateArrowDirection.Horizontal));
            Children.Add(new ArrowTranslateSceneWidget(this, TranslateArrowDirection.Vertical));
        }

        private void Translate(Vector2 translation)
        {
            Position = Position + translation;
            if (OnTranslate == null) return;
            OnTranslate(translation);
        }

        private void TranslationComplete()
        {
            if (OnTranslationComplete == null) return;
            OnTranslationComplete(_startDragPositon, Position);
        }
    }
}
