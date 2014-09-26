using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeUI;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.SceneWidgets.Transformation
{
    public class RotateSceneWidget : BaseWidget
    {
        private RotateWidgetRenderableAsset _asset;
        private Circle Intersection
        {
            get
            {
                return new Circle()
                {
                    Center = _asset.BoundingBox.Center + Position,
                    Radius = _asset.BoundingBox.Width / 2
                };
            }
        }

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

        public override bool Intersects(Vector2 point)
        {
            return Intersection.Contains(point);
        }
    }
}
