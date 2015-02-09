using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using Puppeteer.Resources;

namespace StoryTimeDevKit.Entities.Actors
{
    public class ArmatureActor : BaseActor
    {
        public ArmatureRenderableAsset ArmatureRenderableAsset
        {
            get
            {
                return RenderableAsset as ArmatureRenderableAsset;
            }
            private set
            {
                if (RenderableAsset == value) return;
                RenderableAsset = value;
            }
        }

        public ArmatureActor()
        {
            ArmatureRenderableAsset = new ArmatureRenderableAsset();
            OnCreated += OnCreatedHandler;
        }

        public void OnCreatedHandler()
        {
            Body = Scene.PhysicalWorld.CreateRectangularBody(1, 1, 1);
        }
    }
}
