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
        private MoveArrowDirection _direction;
        private MoveWidgetRenderableAsset _moveWidgetAsset;

        public event Action<MoveArrowSceneWidget, Vector2> OnStartTranslate;
        public event Action<MoveArrowSceneWidget, Vector2, Vector2> OnTranslate;
        public event Action<MoveArrowSceneWidget, Vector2, Vector2, Vector2> OnStopTranslate;

        public MoveArrowDirection Direction { get { return _direction; } }

        public MoveArrowSceneWidget(ActorWidgetAdapter actor, MoveWidgetRenderableAsset moveWidgetAsset, MoveArrowDirection direction)
        {
            _direction = direction;
            _moveWidgetAsset = moveWidgetAsset;
            _actor = actor;
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
            if (OnStartTranslate != null) OnStartTranslate(this, currentPosition);
        }

        private void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
        {
            if (_direction == MoveArrowDirection.Horizontal)
            {
                dragged.Y = 0.0f;
            }
            else
            {
                dragged.X = 0.0f;
            }
            if (OnTranslate != null) OnTranslate(this, dragged, currentPosition);
        }

        private void OnStopDragHandler(Vector2 startDrag, Vector2 currentPosition)
        {
            Vector2 totalTranslation = currentPosition - startDrag;
            if (_direction == MoveArrowDirection.Horizontal)
            {
                totalTranslation.Y = 0;
            }
            else
            {
                totalTranslation.X = 0;
            }
            if (OnStopTranslate != null) OnStopTranslate(this, startDrag, currentPosition, totalTranslation);
        }
    }
}
