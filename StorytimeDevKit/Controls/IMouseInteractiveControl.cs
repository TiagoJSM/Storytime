using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Delegates;

namespace StoryTimeDevKit.Controls
{
    public interface IMouseInteractiveControl
    {
        event OnMouseMove OnMouseMove;
        event OnMouseClick OnMouseClick;
        event OnMouseDown OnMouseDown;
        event OnMouseUp OnMouseUp;
    }
}
