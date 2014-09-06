using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.SceneWidgets.Interfaces
{
    public class RotateSceneWidget : BaseSceneWidget
    {
        private ISceneViewerController _controller; 
        private ActorWidgetAdapter _actor;
        private RotateWidgetRenderableAsset _asset;
        private float _startRotationAngle;
        private float _lastRotationAngle;

        public event Action<float> OnStartRotate;
        public event Action<float> OnRotate;
        public event Action<float> OnStopRotate;

        public RotateSceneWidget(ISceneViewerController controller, ActorWidgetAdapter actor, RotateWidgetRenderableAsset asset)
        {
            _controller = controller;
            _actor = actor;
            _asset = asset;

            OnStartRotate += OnStartRotateHandler;
            OnRotate += OnRotateHandler;
            OnStopRotate += OnStopRotateHandler;

            OnStartDrag += OnStartDragHandler;
            OnDrag += OnDragHandler;
            OnStopDrag += OnStopDragHandler;
        }

        public override bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        private AxisAlignedBoundingBox2D BoundingBox
        {
            get
            {
                AxisAlignedBoundingBox2D box = _asset.BoundingBox;
                box.Translate(_actor.Body.Position);
                return box;
            }
        }

        protected override Vector2 Origin
        {
            get { return Vector2.Zero; }
        }

        private void OnStartRotateHandler(float angle)
        {
        }

        private void OnRotateHandler(float rotation)
        {
            _actor.Body.Rotation += rotation;
        }

        private void OnStopRotateHandler(float angle)
        {
        }

        private void OnStartDragHandler(Vector2 currentPosition)
        {
            _startRotationAngle = currentPosition.AngleWithCenterIn(_actor.Body.Position);
            _lastRotationAngle = _startRotationAngle;
            if (OnStartRotate != null) OnStartRotate(_startRotationAngle);
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            float currentAngle = currentPosition.AngleWithCenterIn(_actor.Body.Position);
            float rotation = currentAngle - _lastRotationAngle;
            _lastRotationAngle = currentAngle;
            if (OnRotate != null) OnRotate(rotation);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            if (OnStopRotate != null) OnStopRotate(currentPosition.AngleWithCenterIn(_actor.Body.Position));
        }
    }
}
