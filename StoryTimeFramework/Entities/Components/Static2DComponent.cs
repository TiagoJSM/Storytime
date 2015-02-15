using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Delegates;
using StoryTimeCore.Extensions;
using StoryTimeCore.General;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Utils;
using StoryTimeFramework.Resources.Graphic;

namespace StoryTimeFramework.Entities.Components
{
    public class Static2DComponent : Component
    {
        private Static2DRenderableAsset _renderableAsset;
        public ITexture2D Texture { get; private set; }

        public string Texture2DName { get; set; }

        public Static2DComponent()
        {
            IsVisible = true;
            OnCreated += OnCreatedHandler;
        }

        protected override void DoRender(IRenderer renderer)
        {
            RenderTexture(renderer, Texture);
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return Texture.GetAABoundingBox(); }
        }

        private void OnCreatedHandler()
        {
            Texture = LoadTexture2D(Texture2DName);
        }
    }
}
