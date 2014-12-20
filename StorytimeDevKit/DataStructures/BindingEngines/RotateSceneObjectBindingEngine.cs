using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.DataStructures.BindingEngines
{
    public class RotateSceneObjectBindingEngine : BindingEngine<RotateSceneWidget, SceneObjectViewModel>
    {
        public RotateSceneObjectBindingEngine(RotateSceneWidget widget , SceneObjectViewModel model)
            : base(widget, model)
        {
            this
                .Bind(tw => tw.Position, a => a.Position)
                .Bind(tw => tw.Rotation, a => a.Rotation)
                .Bind(tw => tw.Active, a => a.RotateWidgetMode)
                .Bind(tw => tw.Visible, a => a.RotateWidgetMode);
        }
    }
}
