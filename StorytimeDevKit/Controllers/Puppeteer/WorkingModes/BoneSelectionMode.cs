﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Entities.Actors;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public class BoneSelectionMode : BaseSelectionMode
    {
        public BoneSelectionMode(IPuppeteerWorkingModeContext context)
            : base(context)
        { 
        }

        public override void OnEnterMode()
        {
            Context.Selected = null;
            base.OnEnterMode();
        }

        protected override void HandleActorIntersection(BaseActor actor, Vector2 position)
        {
            var bone = actor as BoneActor;
            Context.Selected = bone;
        }
    }
}
