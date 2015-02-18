using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using Puppeteer.Resources;
using Puppeteer.Entities;

namespace StoryTimeDevKit.Entities.Actors
{
    public class ArmatureActor : BaseActor
    {
        public SkeletonComponent SkeletonComponent { get; private set; }

        public ArmatureActor()
        {
            OnCreated += OnCreatedHandler;
        }

        public void OnCreatedHandler()
        {
            Body = Scene.PhysicalWorld.CreateRectangularBody(1, 1, 1);
            SkeletonComponent = Components.AddComponent<SkeletonComponent>();
        }
    }
}
