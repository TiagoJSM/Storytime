using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeFramework.WorldManagement;
using Microsoft.Xna.Framework;
using StoryTimeFramework.Entities.Actors;
using Puppeteer.Resources;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public interface IPuppeteerWorkingModeContext
    {
        object Selected { get; set; }
        Scene Scene { get; }

        BoneActor AddBone(Vector2 boneStartPosition);
        BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition);
        BaseActor GetIntersectedActor(Vector2 position);
        void EnableTransformationUI(bool enable);
    }
}
