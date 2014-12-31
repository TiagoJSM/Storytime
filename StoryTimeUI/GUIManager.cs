using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeUI.Interfaces;

namespace StoryTimeUI
{
    public class GUIManager : IParent
    {
        private readonly IGraphicsContext _context;
        private readonly ObservableCollection<BaseWidget> _widgets;
        private BaseWidget _intersected;

        private bool _isDragging;
        private Vector2 _lastDrag;
        private Vector2 _startDrag;

        public GUIManager(IGraphicsContext context)
        {
            _widgets = new ObservableCollection<BaseWidget>();
            _context = context;
            _widgets.CollectionChanged += CollectionChangedHandler;
        }

        public Vector2 Position { get; set; }

        public ObservableCollection<BaseWidget> Children
        {
            get { return _widgets; }
        }

        public void Render(IRenderer renderer)
        {
            foreach (var widget in _widgets)
                widget.Render(renderer);
        }

        public BaseWidget Intersect(Vector2 point)
        {
            var intersected = GetIntersectedLeafChildren(this, point);
            if (intersected == this) return null;
            return intersected as BaseWidget;
        }

        public void MouseDown(Vector2 point)
        {
            _intersected = Intersect(point);
            if (_intersected == null) return;
            _startDrag = point;
            _lastDrag = point;
            _intersected.MouseDown(point);
        }

        public void MouseUp(Vector2 point)
        {
            if (_intersected != null)
            {
                _isDragging = false;
                _intersected.StopDrag(point);
                _intersected.MouseUp(point);
            }
            _intersected = null;
        }

        public void MouseMove(Vector2 point)
        {
            if (_intersected == null) return;
            if (!_isDragging)
            {
                _intersected.StartDrag(_startDrag);
                _isDragging = true;
            }
            var dragged = point - _lastDrag;
            _lastDrag = point;
            _intersected.Drag(dragged, point);
        }

        private IParent GetIntersectedLeafChildren(IParent parent, Vector2 point)
        {
            IParent intersected = null;
            var children = parent.Children;
            for (var idx = children.Count - 1; idx >= 0; idx--)
            {
                if (!children[idx].Active) continue;

                if (children[idx].Intersects(point))
                {
                    intersected = children[idx];
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
                foreach (var newItem in e.NewItems)
                {
                    var widget = newItem as BaseWidget;
                    widget.Parent = this;
                    widget.GraphicsContext = _context;
                    widget.Initialize();
                }
            }
        }
    }
}