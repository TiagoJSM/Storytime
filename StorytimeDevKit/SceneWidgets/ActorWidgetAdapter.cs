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

namespace StoryTimeDevKit.SceneWidgets.Interfaces
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
                else
                {
                    _rotateWidgetAsset.Render(renderer);
                }
            }

            private void OnSelectHandler(bool selected)
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
        private ISceneViewerController _controller;
        private WidgetMode _widgetMode;
        private Vector2 _startDrag;

        public event Action<Vector2> OnStartDrag;
        public event Action<Vector2, Vector2> OnDrag;
        public event Action<Vector2, Vector2> OnStopDrag;

        public event Action<bool> OnSelect;
        public event Action<bool> OnEnabled;
        public event Action<WidgetMode> OnWidgetModeChange;

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
                        OnSelect(value);
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

        public ActorWidgetAdapter(ISceneViewerController controller, BaseActor ba, IGraphicsContext graphicsContext)
        {
            _controller = controller;
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

            _horizontalArrow = new MoveArrowSceneWidget(_controller, this, _moveAsset, MoveArrowSceneWidget.MoveArrowDirection.Horizontal);
            _verticalArrow = new MoveArrowSceneWidget(_controller, this, _moveAsset, MoveArrowSceneWidget.MoveArrowDirection.Vertical);
            _rotationWheel = new RotateSceneWidget(_controller, this, _rotateAsset);
            _controller = controller;
            _enabled = true;
            WidgetMode = WidgetMode.Rotate;
            SetSceneWidgetsEnableStatus();
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
    }
}
