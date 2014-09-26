using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeUI.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StoryTimeUI
{
    public class GUIManager : IParent
    {
        private ObservableCollection<BaseWidget> _widgets;
        private IGraphicsContext _context;
        private BaseWidget _intersected;

        private bool _isDragging;
        private Vector2 _startDrag;

        public GUIManager(IGraphicsContext context)
        {
            _widgets = new ObservableCollection<BaseWidget>();
            _context = context;
            _widgets.CollectionChanged += CollectionChangedHandler;
        }

        public void Render(IRenderer renderer)
        {
            foreach (BaseWidget widget in _widgets)
                widget.Render(renderer);
        }

        public ObservableCollection<BaseWidget> Children { get { return _widgets; } }

        //public void Add(BaseWidget widget)
        //{
        //    widget.Parent = this;
        //    widget.GraphicsContext = _context;
        //    widget.Initialize();
        //    _widgets.Add(widget);
        //}

        public BaseWidget Intersect(Vector2 point)
        {
            IParent intersected = GetIntersectedLeafChildren(this, point);
            if (intersected == this) return null;
            return intersected as BaseWidget;
        }

        public void MouseDown(Vector2 point)
        {
            _intersected = Intersect(point);
            _startDrag = point;
            _intersected.MouseDown(point);
        }

        public void MouseUp(Vector2 point)
        {
            if (_intersected != null)
            {
                _isDragging = false;
                _intersected.StopDrag(point);
            }
            _intersected = Intersect(point);
            _intersected.MouseUp(point);
        }

        public void MouseMove(Vector2 point)
        {
            if (_intersected == null) return;
            if (!_isDragging)
            {
                _intersected.StartDrag(_startDrag);
                _isDragging = true;
            }
            Vector2 dragged = new Vector2(
                point.X - _startDrag.X,
                point.Y - _startDrag.Y);
            _intersected.Drag(dragged, point);
        }

        private IParent GetIntersectedLeafChildren(IParent parent, Vector2 point)
        {
            IParent intersected = null;
            ObservableCollection<BaseWidget> children = parent.Children;
            for (int idx = children.Count - 1; idx >= 0; idx--)
            {
                if (_widgets[idx].Intersects(point))
                {
                    intersected = _widgets[idx];
                    break;
                }
            }
            if (intersected == null) return parent;
            return GetIntersectedLeafChildren(intersected, point);
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object newItem in e.NewItems)
                {
                    BaseWidget widget = newItem as BaseWidget;
                    widget.Parent = this;
                    widget.GraphicsContext = _context;
                    widget.Initialize();
                }
            }
        }
    }
}
