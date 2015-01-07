using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeUI.DataBinding.Engines;

namespace StoryTimeDevKit.DataStructures.BindingEngines
{
    public class FreeMovementSceneObjectBindingEngine : BindingEngine<FreeMovementSceneWidget, SceneObjectViewModel>
    {
        public FreeMovementSceneObjectBindingEngine(FreeMovementSceneWidget widget, SceneObjectViewModel model)
            : base(widget, model)
        {
            this
                .Bind(tw => tw.Position, a => a.Position)
                .Bind(tw => tw.Active, a => a.FreeMovementWidgetMode)
                .Bind(tw => tw.Visible, a => a.FreeMovementWidgetMode);
        }
    }
}
