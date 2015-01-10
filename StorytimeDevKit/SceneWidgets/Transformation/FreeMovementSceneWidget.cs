using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.Renderables;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public class FreeMovementSceneWidget : BaseWidget
    {
        private FreeMovementRenderableAsset _asset;
        private Vector2 _startDragPositon;

        public event OnTranslate OnTranslate;
        public event OnTranslationComplete OnTranslationComplete;

        public FreeMovementSceneWidget()
        {
            OnInitialize += OnInitializeHandler;
            OnStartDrag += OnStartDragHandler;
            OnDrag += OnDragHandler;
            OnStopDrag += OnStopDragHandler;
        }

        protected override IRenderableAsset RenderableAsset
        {
            get { return _asset; }
        }

        private void OnInitializeHandler()
        {
            _asset = new FreeMovementRenderableAsset(GraphicsContext);
        }

        private void OnStartDragHandler(Vector2 currentPosition)
        {
            _startDragPositon = Position;
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            Position = Position + dragged;
            if (OnTranslate == null) return;
            OnTranslate(dragged);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            if (OnTranslationComplete == null) return;
            OnTranslationComplete(_startDragPositon, Position);
        }

        public override bool Intersects(Vector2 point)
        {
            var box = _asset.AABoundingBox;
            box.Translate(point);
            return box.Contains(point);
        }
    }
}
