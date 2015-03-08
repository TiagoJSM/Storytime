using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.Renderables;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public delegate void OnStartRotation(float originalRotation);
    public delegate void OnRotation(float rotation);
    public delegate void OnStopRotation(float originalRotation, float finalRotation, float totalRotation);

    public class RotateSceneWidget : BaseWidget
    {
        private RotateWidgetRenderableAsset _asset;
        private float _startRotation;
        private float _lastAngle;
        private float _originalRotation;
        private float _totalRotation;

        private Circle Intersection
        {
            get
            {
                return new Circle()
                {
                    Center = _asset.AABoundingBox.Center + Position,
                    Radius = _asset.AABoundingBox.Width / 2
                };
            }
        }

        public OnStartRotation OnStartRotation;
        public OnRotation OnRotation;
        public OnStopRotation OnStopRotation;

        public RotateSceneWidget()
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
            _asset = new RotateWidgetRenderableAsset(GraphicsContext);
        }

        public override bool Intersects(Vector2 point)
        {
            return Intersection.Contains(point);
        }

        private void OnStartDragHandler(Vector2 currentPosition)
        {
            _lastAngle = currentPosition.AngleWithCenterIn(Position);
            _originalRotation = Rotation;
            _totalRotation = 0;
            if (OnStartRotation != null)
                OnStartRotation(_originalRotation);
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            var currentAngle = currentPosition.AngleWithCenterIn(Position);
            var rotation = currentAngle - _lastAngle;
            _lastAngle = currentAngle;
            _totalRotation += rotation;
            Rotation = Rotation + rotation;
            if (OnRotation != null)
                OnRotation(rotation);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            if (OnStopRotation != null)
                OnStopRotation(_originalRotation, Rotation, _totalRotation);
        }
    }
}
