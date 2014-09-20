using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public class RotateSceneWidget : BaseWidget
    {
        private RotateWidgetRenderableAsset _asset;
        public RotateSceneWidget()
        {
            OnInitialize += OnInitializeHandler;
        }

        protected override IRenderableAsset RenderableAsset
        {
            get { return _asset; }
        }

        private void OnInitializeHandler()
        {
            _asset = new RotateWidgetRenderableAsset(GraphicsContext);
        }
    }
}
