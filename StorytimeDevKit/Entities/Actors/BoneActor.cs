using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Entities.Actors
{
    public class BoneActor : BaseActor
    {
        public BoneActor()
        {
            OnCreated += OnCreatedHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        private void OnCreatedHandler()
        {
            ITexture2D bitmap = Scene.GraphicsContext.LoadTexture2D("Bone");
            Static2DRenderableAsset asset = new Static2DRenderableAsset();
            asset.Texture2D = bitmap;
            asset.Origin = new Vector2(bitmap.Width / 2, 0.0f);
            RenderableAsset = asset;
        }
    }
}
