using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.SceneWidgets.Interfaces
{
    public enum WidgetMode
    {
        Translate,
        Rotate,
        Scale
    }

    public interface ITransformableWidget : ISceneWidget
    {
        WidgetMode WidgetMode { get; set; }
    }
}
