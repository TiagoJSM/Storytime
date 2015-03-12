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
using StoryTimeCore.Utils;

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
        private Vector2 _lastPosition;

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
            _lastPosition = currentPosition;
            _lastAngle = currentPosition.AngleWithCenterIn(Position);
            _originalRotation = Rotation;
            _totalRotation = 0;
            if (OnStartRotation != null)
                OnStartRotation(_originalRotation);
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            var clockwise = MathematicalUtils.IsClockwiseRotation(_lastPosition, currentPosition, Position);

            var angle = MathematicalUtils.AngleBetween(_lastPosition, currentPosition, Position);
            if (clockwise)
            {
                angle = -angle;
            }
            _lastPosition = currentPosition;
            var currentAngle = currentPosition.AngleWithCenterIn(Position);
            
            _lastAngle = currentAngle;
            _totalRotation += angle;
            Rotation = Rotation + angle;

            if (OnRotation != null)
                OnRotation(angle);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            if (OnStopRotation != null)
                OnStopRotation(_originalRotation, Rotation, _totalRotation);
        }
    }
}
