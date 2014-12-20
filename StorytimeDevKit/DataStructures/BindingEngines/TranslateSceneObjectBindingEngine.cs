using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.DataStructures.BindingEngines
{
    public class TranslateSceneObjectBindingEngine : BindingEngine<TranslateSceneWidget, SceneObjectViewModel>
    {
        public TranslateSceneObjectBindingEngine(TranslateSceneWidget widget, SceneObjectViewModel model)
            :base(widget, model)
        {
            this
                .Bind(tw => tw.Position, a => a.Position)
                .Bind(tw => tw.Active, a => a.TranslateWidgetMode)
                .Bind(tw => tw.Visible, a => a.TranslateWidgetMode);
        }
    }
}
