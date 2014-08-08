using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.SceneWidgets
{
    public abstract class BaseSceneWidget : ISceneWidget
    {
        private bool _selected;
        private Vector2 _startDrag;

        public event Action<Vector2> OnStartDrag;
        public event Action<Vector2, Vector2> OnDrag;
        public event Action<Vector2, Vector2> OnStopDrag;
        public event Action<bool> OnSelect;

        public BaseSceneWidget()
        {
        }

        public WidgetMode WidgetMode { get; set; }

        public virtual IEnumerable<ISceneWidget> Children { get { return new List<ISceneWidget>(); } }

        public virtual int ChildrenCount { get { return 0; } }

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

        public bool Intersects(Vector2 point)
        {
            return BoundingBox.Contains(point);
        }

        public void StartDrag(Vector2 currentPosition)
        {
            if (OnStartDrag != null)
                OnStartDrag(currentPosition);
            _startDrag = currentPosition;
        }

        public virtual void Drag(Vector2 dragged, Vector2 currentPosition)
        {
            if (OnDrag != null)
                OnDrag(dragged, currentPosition);
        }

        public virtual void StopDrag(Vector2 currentPosition)
        {
            if (OnStopDrag != null)
                OnStopDrag(_startDrag, currentPosition);
        }

        protected abstract Rectanglef BoundingBox { get; }
    }
}
