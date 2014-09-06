using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.SceneWidgets.Interfaces
{
    public class MoveArrowSceneWidget : BaseSceneWidget
    {
        public enum MoveArrowDirection
        {
            Vertical,
            Horizontal
        }

        private ActorWidgetAdapter _actor;
        private Vector2 _startDragPosition;
        private MoveArrowDirection _direction;
        private ISceneViewerController _controller;
        private MoveWidgetRenderableAsset _moveWidgetAsset;

        public event Action<Vector2> OnStartTranslate;
        public event Action<Vector2, Vector2> OnTranslate;
        public event Action<Vector2, Vector2> OnStopTranslate;

        public MoveArrowSceneWidget(ISceneViewerController controller, ActorWidgetAdapter actor, MoveWidgetRenderableAsset moveWidgetAsset, MoveArrowDirection direction)
        {
            _controller = controller;
            _actor = actor;
            _direction = direction;
            _moveWidgetAsset = moveWidgetAsset;

            OnStartTranslate += OnStartTranslateHandler;
            OnTranslate += OnTranslateHandler;
            OnStopTranslate += OnStopTranslateHandler;

            OnStartDrag += OnStartDragHandler;
            OnDrag += OnDragHandler;
            OnStopDrag += OnStopDragHandler;
        }

        public void OnStartTranslateHandler(Vector2 currentPosition)
        {
            _startDragPosition = _actor.Body.Position;
        }

        public void OnTranslateHandler(Vector2 dragged, Vector2 currentPosition)
        {
            if (_direction == MoveArrowDirection.Horizontal)
            {
                dragged.Y = 0.0f;
                currentPosition.Y = _startDragPosition.Y;
            }
            else
            {
                dragged.Y = 0.0f;
                currentPosition.X = _startDragPosition.X;
            }
            _actor.Body.Position = currentPosition;
        }

        public override bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        private void OnStopTranslateHandler(Vector2 startDragPosition, Vector2 currentPosition)
        {
            if (_direction == MoveArrowDirection.Horizontal)
            {
                currentPosition.Y = startDragPosition.Y;
            }
            else
            {
                currentPosition.X = startDragPosition.X;
            }
            _controller.MoveActor(_actor.BaseActor, _startDragPosition, currentPosition);
        }

        private AxisAlignedBoundingBox2D BoundingBox
        {
            get
            {
                AxisAlignedBoundingBox2D box;
                if (_direction == MoveArrowDirection.Horizontal)
                {
                    box = _moveWidgetAsset.HorizontalArrowBox;
                }
                else
                {
                    box = _moveWidgetAsset.VerticalArrowBox;
                }
                box.Translate(_actor.Body.Position);
                return box;
            }
        }

        protected override Vector2 Origin
        {
            get { return _actor.Body.Position; }
        }

        private void OnStartDragHandler(Vector2 currentPosition)
        {
            if (OnStartTranslate != null) OnStartTranslate(currentPosition);
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            if (OnTranslate != null) OnTranslate(dragged, currentPosition);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            if (OnStopTranslate != null) OnStopTranslate(startDrag, currentPosition);
        }
    }
}
