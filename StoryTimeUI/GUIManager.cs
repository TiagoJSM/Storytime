using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeUI.Interfaces;

namespace StoryTimeUI
{
    public class GUIManager : IParent
    {
        private List<BaseWidget> _widgets;
        private IGraphicsContext _context;

        public GUIManager(IGraphicsContext context)
        {
            _widgets = new List<BaseWidget>();
            _context = context;
        }

        public void Render(IRenderer renderer)
        {
            foreach (BaseWidget widget in _widgets)
                widget.Render(renderer);
        }

        public void Add(BaseWidget widget)
        {
            widget.Parent = this;
            widget.GraphicsContext = _context;
            widget.Initialize();
            _widgets.Add(widget);
        }
    }
}
