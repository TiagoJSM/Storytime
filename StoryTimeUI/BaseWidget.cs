using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeUI.Interfaces;
using System.Collections.ObjectModel;
using StoryTimeUI.Delegates;
using System.Collections.Specialized;

namespace StoryTimeUI
{
    public abstract class BaseWidget : IParent
    {
        private IGraphicsContext _graphicsContext;
        private bool _initialized;
        private ObservableCollection<BaseWidget> _children;
        private Vector2 _startDrag;

        public event Action OnInitialize;
        public event OnStartDrag OnStartDrag;
        public event OnDrag OnDrag;
        public event OnStopDrag OnStopDrag;
        public event OnMouseDown OnMouseDown;
        public event OnMouseUp OnMouseUp;

        public IGraphicsContext GraphicsContext
        {  
            get 
            {
                return _graphicsContext;
            }
            set 
            {
                if(_graphicsContext == null)
                    _graphicsContext = value;
            }
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public IParent Parent { get; set; }
        public bool Active { get; set; }
        public bool Visible { get; set; }
        public ObservableCollection<BaseWidget> Children { get { return _children; } }

        protected virtual IRenderableAsset RenderableAsset { get { return null; } }

        public BaseWidget()
        {
            Active = true;
            Visible = true;
            _children = new ObservableCollection<BaseWidget>();
            _children.CollectionChanged += CollectionChangedHandler;
        }

        public void Render(IRenderer renderer)
        {
            if (!Visible) return;
            renderer.TranslationTransformation += Position;
            renderer.RotationTransformation += Rotation;
            
            if(RenderableAsset != null)
                RenderableAsset.Render(renderer);
            foreach (BaseWidget child in Children)
                child.Render(renderer);

            renderer.RotationTransformation -= Rotation;
            renderer.TranslationTransformation -= Position;
        }

        public void Initialize()
        {
            if (_initialized) return;
            if (OnInitialize != null)
                OnInitialize();
            _initialized = true;
        }

        public void StartDrag(Vector2 currentPosition)
        {
            if (!Active) return;
            if (OnStartDrag != null) 
                OnStartDrag(currentPosition);
            _startDrag = currentPosition;
        }

        public void Drag(Vector2 dragged, Vector2 currentPosition)
        {
            if (!Active) return;
            if (OnDrag != null) 
                OnDrag(dragged, currentPosition);
        }

        public void StopDrag(Vector2 currentPosition)
        {
            if (!Active) return;
            if (OnStopDrag != null) 
                OnStopDrag(_startDrag, currentPosition);
        }

        public void MouseDown(Vector2 point)
        {
            if (!Active) return;
            if (OnMouseDown != null)
                OnMouseDown(point);
        }

        public void MouseUp(Vector2 point)
        {
            if (!Active) return;
            if (OnMouseUp != null)
                OnMouseUp(point);
        }

        public abstract bool Intersects(Vector2 point);

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object newItem in e.NewItems)
                {
                    BaseWidget widget = newItem as BaseWidget;
                    widget.Parent = this;
                    widget.GraphicsContext = GraphicsContext;
                    widget.Initialize();
                }
            }
        }
    }
}
