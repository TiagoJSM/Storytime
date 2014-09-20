using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Entities.SceneWidgets.Interfaces
{
    public enum WidgetMode
    {
        None,
        Translate,
        Rotate,
        Scale
    }

    public interface ITransformableWidget : ISceneWidget
    {
        WidgetMode WidgetMode { get; set; }
    }
}
