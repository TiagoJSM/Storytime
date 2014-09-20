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
using StoryTimeCore.Extensions;
using StoryTimeDevKit.Controllers;
using StoryTimeDevKit.Delegates;
using StoryTimeFramework.Resources.Graphic;

namespace StoryTimeDevKit.Entities.SceneWidgets.Interfaces
{
    public class ActorWidgetAdapter : BaseActor, ITransformableWidget
    {
        private class TransformationActorRenderableAsset : TemplateRenderableAsset
        {
            private MoveWidgetRenderableAsset _moveWidgetAsset;
            private RotateWidgetRenderableAsset _rotateWidgetAsset;
            private ActorWidgetAdapter _adapter;
            private IRenderableAsset _asset;

            public TransformationActorRenderableAsset(
                ActorWidgetAdapter adapter, 
                IRenderableAsset asset, 
                MoveWidgetRenderableAsset moveWidgetAsset,
                RotateWidgetRenderableAsset rotateWidgetAsset)
            {
                _adapter = adapter;
                _asset = asset;
                _moveWidgetAsset = moveWidgetAsset;
                _rotateWidgetAsset = rotateWidgetAsset;

                _adapter.OnSelect += OnSelectHandler;
                _adapter.OnWidgetModeChange += OnWidgetModeChangeHandler;
            }

            protected override AxisAlignedBoundingBox2D RawBoundingBox
            {
                get
                {
                    AxisAlignedBoundingBox2D box = _asset.BoundingBox;
                    if (!_adapter.Selected)
                        return box;
                    
                    if(_adapter.WidgetMode == WidgetMode.Translate)
                        return box.Combine(_moveWidgetAsset.BoundingBox);
                    else
                        return box.Combine(_rotateWidgetAsset.BoundingBox);
                }
            }

            public override void Render(IRenderer renderer)
            {
                _asset.Render(renderer);
                if (!_adapter.Selected)
                    return;

                if (_adapter.WidgetMode == WidgetMode.Translate)
                {
                    float rendererRotation = renderer.RotationTransformation;
                    renderer.RotationTransformation = 0;
                    _moveWidgetAsset.Render(renderer);
                    renderer.RotationTransformation = rendererRotation;
                }
                else if(_adapter.WidgetMode == WidgetMode.Rotate)
                {
                    _rotateWidgetAsset.Render(renderer);
                }
            }

            private void OnSelectHandler(bool selected, ISceneWidget widget)
            {
                RaiseOnBoundingBoxChanges();
            }
            private void OnWidgetModeChangeHandler(WidgetMode mode)
            {
                RaiseOnBoundingBoxChanges();
            }
        }

        private Vector2 _startDragPosition;
        private MoveWidgetRenderableAsset _moveAsset;
        private RotateWidgetRenderableAsset _rotateAsset;
        private bool _selected;
        private bool _enabled;
        private MoveArrowSceneWidget _horizontalArrow;
        private MoveArrowSceneWidget _verticalArrow;
        private RotateSceneWidget _rotationWheel;
        private WidgetMode _widgetMode;
        private Vector2 _startDrag;
        private float _bodyStartRotation;

        public event OnStartDrag OnStartDrag;
        public event OnDrag OnDrag;
        public event OnStopDrag OnStopDrag;

        public event Action<bool, ISceneWidget> OnSelect;
        public event Action<bool> OnEnabled;
        public event Action<WidgetMode> OnWidgetModeChange;

        public event OnTranslated OnTranslated;
        public event OnRotated OnRotated;

        public BaseActor BaseActor { get; private set; }
        public WidgetMode WidgetMode 
        {
            get 
            { 
                return _widgetMode; 
            }
            set
            {
                if (_widgetMode != value)
                {
                    _widgetMode = value;
                    SetSceneWidgetsEnableStatus();
                    if (OnWidgetModeChange != null) OnWidgetModeChange(_widgetMode);
                }   
            }
        }
        public IEnumerable<ISceneWidget> Children
        {
            get 
            {
                return new List<ISceneWidget>() 
                { 
                    _horizontalArrow, 
                    _verticalArrow,
                    _rotationWheel
                }; 
            }
        }

        public int ChildrenCount
        {
            get { return 3; }
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
                        OnSelect(value, this);
                }
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (OnEnabled != null)
                        OnEnabled(value);
                }
            }
        }

        public ActorWidgetAdapter(BaseActor ba, IGraphicsContext graphicsContext)
        {
            _moveAsset = new MoveWidgetRenderableAsset(graphicsContext);
            _rotateAsset = new RotateWidgetRenderableAsset(graphicsContext);
            BaseActor = ba;
            Body = ba.Body;
            RenderableAsset = new TransformationActorRenderableAsset(
                this,
                ba.RenderableAsset,
                _moveAsset,
                _rotateAsset
            );

            _horizontalArrow = new MoveArrowSceneWidget(this, _moveAsset, MoveArrowSceneWidget.MoveArrowDirection.Horizontal);
            _verticalArrow = new MoveArrowSceneWidget(this, _moveAsset, MoveArrowSceneWidget.MoveArrowDirection.Vertical);
            _rotationWheel = new RotateSceneWidget(this, _rotateAsset);
            _enabled = true;

            SetSceneWidgetsEnableStatus();

            _horizontalArrow.OnStartTranslate += OnStartTranslateHandler;
            _horizontalArrow.OnTranslate += OnTranslateHandler;
            _horizontalArrow.OnStopTranslate += OnStopTranslateHandler;

            _verticalArrow.OnStartTranslate += OnStartTranslateHandler;
            _verticalArrow.OnTranslate += OnTranslateHandler;
            _verticalArrow.OnStopTranslate += OnStopTranslateHandler;

            _rotationWheel.OnStartRotate += OnStartRotateHandler;
            _rotationWheel.OnRotate += OnRotateHandler;
            _rotationWheel.OnStopRotate += OnStopRotateHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
            
        }

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        public void StartDrag(Vector2 currentPosition)
        {
            if (OnStartDrag != null) OnStartDrag(currentPosition);
            _startDrag = currentPosition;
        }

        public void Drag(Vector2 dragged, Vector2 currentPosition)
        {
            if (OnDrag != null) OnDrag(dragged, currentPosition);
        }

        public void StopDrag(Vector2 currentPosition)
        {
            if (OnStopDrag != null) OnStopDrag(_startDrag, currentPosition);
        }

        private void SetSceneWidgetsEnableStatus()
        {
            _horizontalArrow.Enabled = _widgetMode == WidgetMode.Translate;
            _verticalArrow.Enabled = _widgetMode == WidgetMode.Translate;
            _rotationWheel.Enabled = _widgetMode == WidgetMode.Rotate;
        }

        #region MoveArrowSceneWidgetEventHandlers
        private void OnStartTranslateHandler(MoveArrowSceneWidget widget, Vector2 currentPosition)
        {
            _startDragPosition = Body.Position;
        }

        private void OnTranslateHandler(MoveArrowSceneWidget widget, Vector2 dragged, Vector2 currentPosition)
        {
            if (widget.Direction == MoveArrowSceneWidget.MoveArrowDirection.Horizontal)
            {
                dragged.Y = 0.0f;
                currentPosition.Y = _startDragPosition.Y;
            }
            else
            {
                dragged.X = 0.0f;
                currentPosition.X = _startDragPosition.X;
            }
            Body.Position = currentPosition;
        }

        private void OnStopTranslateHandler(
            MoveArrowSceneWidget widget, Vector2 startDragPosition, Vector2 currentPosition, Vector2 totalTranslation)
        {
            if (widget.Direction == MoveArrowSceneWidget.MoveArrowDirection.Horizontal)
            {
                currentPosition.Y = startDragPosition.Y;
            }
            else
            {
                currentPosition.X = startDragPosition.X;
            }

            if (OnTranslated != null)
                OnTranslated(BaseActor, _startDragPosition, currentPosition);
        }
        #endregion

        #region RotateSceneWidgetEventHandlers
        private void OnStartRotateHandler(float angle)
        {
            _bodyStartRotation = Body.Rotation;
        }

        private void OnRotateHandler(float rotation)
        {
            Body.Rotation += rotation;
        }

        private void OnStopRotateHandler(float angle)
        {
            if (OnRotated != null)
                OnRotated(BaseActor, _bodyStartRotation, Body.Rotation);
            //_controller.RotateActor(BaseActor, _bodyStartRotation, Body.Rotation);
        }
        #endregion
    }
}
