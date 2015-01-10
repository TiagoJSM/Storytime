using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Entities.Renderables;

namespace StoryTimeDevKit.DataStructures
{
    public class SceneControlData
    {
        private TransformModeViewModel _transformModeModel;

        public TranslateSceneWidget TranslateSceneWidget { get; private set; }
        public RotateSceneWidget RotateSceneWidget { get; private set; }
        public BindingEngine<TranslateSceneWidget, TransformActorViewModel> TranslateBindingEngine { get; private set; }
        public BindingEngine<RotateSceneWidget, TransformActorViewModel> RotateBindingEngine { get; private set; }
        public TransformActorViewModel TransformActorModel { get; private set; }

        public SceneControlData(
            TranslateSceneWidget translateWidg, RotateSceneWidget rotateWidg, TransformModeViewModel transformModeModel)
        {
            TransformActorModel = new TransformActorViewModel();
            TranslateSceneWidget = translateWidg;
            RotateSceneWidget = rotateWidg;

            TranslateBindingEngine =
                    new BindingEngine<TranslateSceneWidget, TransformActorViewModel>(TranslateSceneWidget, TransformActorModel)
                        .Bind(tw => tw.Position, a => a.Position, BindingType.TwoWays)
                        .Bind(tw => tw.Active, a => a.TranslateWidgetMode)
                        .Bind(tw => tw.Visible, a => a.TranslateWidgetMode);

            RotateBindingEngine =
                    new BindingEngine<RotateSceneWidget, TransformActorViewModel>(RotateSceneWidget, TransformActorModel)
                        .Bind(tw => tw.Position, a => a.Position, BindingType.TwoWays)
                        .Bind(tw => tw.Rotation, a => a.Rotation, BindingType.TwoWays)
                        .Bind(tw => tw.Active, a => a.RotateWidgetMode)
                        .Bind(tw => tw.Visible, a => a.RotateWidgetMode);

            _transformModeModel = transformModeModel;
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
            TransformActorModel.WidgetMode = _transformModeModel.WidgetMode;
        }

        private void OnWidgetModeChanges(WidgetMode mode)
        {
            TransformActorModel.WidgetMode = mode;
        }

        public void UnassignEvents()
        {
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
        }
    }
}
