using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.SceneWidgets.Interfaces
{
    public abstract class BaseSceneWidget : ISceneWidget
    {
        private bool _selected;
        private bool _enabled;
        private Vector2 _startDrag;

        public event Action<Vector2> OnStartDrag;
        public event Action<Vector2, Vector2> OnDrag;
        public event Action<Vector2, Vector2> OnStopDrag;

        public event Action<bool> OnSelect;
        public event Action<bool> OnEnabled;

        public BaseSceneWidget()
        {
            _enabled = true;
        }

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

        public abstract bool Intersects(Vector2 point);
        protected abstract Vector2 Origin { get; }
    }
}
