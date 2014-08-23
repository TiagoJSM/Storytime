using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeDevKit.Controllers.Scenes;

namespace StoryTimeDevKit.SceneWidgets
{
    public class ActorWidgetAdapter : BaseActor, ISceneWidget
    {
        private class MoveArrowSceneWidget : BaseSceneWidget
        {
            public enum MoveArrowDirection
            {
                Vertical,
                Horizontal
            }

            private ActorWidgetAdapter _actor;
            private MoveWidgetRenderableAsset _moveWidget;
            private Vector2 _startDragPosition;
            private MoveArrowDirection _direction;

            public MoveArrowSceneWidget(ActorWidgetAdapter actor, MoveWidgetRenderableAsset moveWidget, MoveArrowDirection direction)
            {
                _actor = actor;
                _moveWidget = moveWidget;
                _direction = direction;
                OnStartDrag += OnStartDragHandler;
                OnDrag += OnDragHandler;
                OnStopDrag += OnStopDragHandler;
            }

            public void OnStartDragHandler(Vector2 currentPosition)
            {
                _startDragPosition = _actor.Body.Position;
            }

            public void OnDragHandler(Vector2 dragged, Vector2 currentPosition)
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

            private void OnStopDragHandler(Vector2 startDragPosition, Vector2 currentPosition)
            {
                if (_direction == MoveArrowDirection.Horizontal)
                {
                    currentPosition.Y = startDragPosition.Y;
                }
                else
                {
                    currentPosition.X = startDragPosition.X;
                }
                _actor._controller.MoveActor(_actor.BaseActor, _startDragPosition, currentPosition);
            }

            protected override Rectanglef BoundingBox
            {
                get 
                {
                    Rectanglef box;
                    if (_direction == MoveArrowDirection.Horizontal)
                    {
                        box = _moveWidget.HorizontalArrowBox;
                    }
                    else
                    {
                        box = _moveWidget.VerticalArrowBox;
                    }
                    box.Translate(_actor.Body.Position);
                    return box;
                }
            }
        }

        private class ActorAdapterRenderableAsset : CompositeRenderableAsset
        {
            private MoveWidgetRenderableAsset _widgetAsset;
            private ActorWidgetAdapter _adapter;
            private IRenderableAsset _asset;

            public ActorAdapterRenderableAsset(ActorWidgetAdapter adapter, IRenderableAsset asset, MoveWidgetRenderableAsset widgetAsset)
                : base(asset, widgetAsset)
            {
                _adapter = adapter;
                _widgetAsset = widgetAsset;
                _asset = asset;

                _adapter.OnSelect += OnSelectHandler;
            }

            public override Rectanglef BoundingBox
            {
                get
                {
                    if (_adapter.Selected)
                        return base.BoundingBox;
                    return _asset.BoundingBox;
                }
            }

            public override void Render(IRenderer renderer)
            {
                foreach (IRenderableAsset asset in Assets)
                {
                    if (asset == _widgetAsset)
                    {
                        if (_adapter.Selected)
                            asset.Render(renderer);
                    }
                    else
                    {
                        asset.Render(renderer);
                    }
                }
            }

            private void OnSelectHandler(bool selected)
            {
                RaiseOnBoundingBoxChanges();
            }
        }

        private Vector2 _startDragPosition;
        private MoveWidgetRenderableAsset _moveWidget;
        private bool _selected;
        private MoveArrowSceneWidget _horizontalArrow;
        private MoveArrowSceneWidget _verticalArrow;
        private ISceneViewerController _controller;

        public event Action<Vector2> OnStartDrag;
        public event Action<Vector2, Vector2> OnDrag;
        public event Action<Vector2, Vector2> OnStopDrag;
        public event Action<bool> OnSelect;

        public BaseActor BaseActor { get; private set; }
        public WidgetMode WidgetMode { get; set; }
        public IEnumerable<ISceneWidget> Children
        {
            get 
            {
                return new List<ISceneWidget>() 
                { 
                    _horizontalArrow, 
                    _verticalArrow
                }; 
            }
        }

        public int ChildrenCount
        {
            get { return 2; }
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    if (OnSelect != null)
                        OnSelect(value);
                }
            }
        }

        public ActorWidgetAdapter(ISceneViewerController controller, BaseActor ba, IGraphicsContext graphicsContext)
        {
            _moveWidget = new MoveWidgetRenderableAsset(graphicsContext);
            BaseActor = ba;
            Body = ba.Body;
            RenderableAsset = new ActorAdapterRenderableAsset(
                this,
                ba.RenderableAsset,
                _moveWidget
            );
            
            _horizontalArrow = new MoveArrowSceneWidget(this, _moveWidget, MoveArrowSceneWidget.MoveArrowDirection.Horizontal);
            _verticalArrow = new MoveArrowSceneWidget(this, _moveWidget, MoveArrowSceneWidget.MoveArrowDirection.Vertical);
            _controller = controller;
        }

        public override void TimeElapse(WorldTime WTime)
        {
            
        }

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        public void StartDrag(Vector2 currentPosition) { }
        public void Drag(Vector2 dragged, Vector2 currentPosition) { }
        public void StopDrag(Vector2 currentPosition) { }
    }
}
