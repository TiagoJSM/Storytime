using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.Entities.Actors
{
    public class BoneActor : BaseActor
    {
        public Vector2 BoneEnd
        {
            get 
            {
                Vector2 position = Body.Position;
                position.Y += RenderableAsset.BoundingBox.Height;
                return position.Rotate(Body.Rotation, Body.Position);
            }
            set
            {
                AxisAlignedBoundingBox2D originalBounds = 
                    RenderableAsset
                    .BoundingBoxWithoutOrigin
                    .GetScaled(RenderableAsset.Scale.Inverse(), Vector2.Zero);

                float distance = Vector2.Distance(BoneEnd, value);
                float angle = value.AngleWithCenterIn(Body.Position) - 90.0f;
                float yScale = distance / originalBounds.Height + 1.0f;
                float xScale = distance / originalBounds.Width + 1.0f;
                Body.Rotation = angle;
                RenderableAsset.Scale = new Vector2(1.0f, yScale);
            }
        }

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
