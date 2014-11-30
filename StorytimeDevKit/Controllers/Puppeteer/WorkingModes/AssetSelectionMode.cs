using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Entities.Actors;
using Puppeteer.Extensions;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public class AssetSelectionMode : BaseSelectionMode
    {
        public AssetSelectionMode(IPuppeteerWorkingModeContext context)
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
            ArmatureActor armature = actor as ArmatureActor;
            if (armature == null) return;
            Context.Selected = 
                armature.ArmatureRenderableAsset.GetIntersectedBoneAttachedAssets(position).FirstOrDefault();
            //armature.ArmatureRenderableAsset.
            //Context.SelectedObject = bone;
        }
    }
}
