using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeFramework.WorldManagement;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public interface IPuppeteerWorkingModeContext
    {
        BoneActor SelectedBone { get; set; }

        BoneActor AddBone(Vector2 boneStartPosition);
        BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition);
        BoneActor GetIntersectedBone(Vector2 position);
        void EnableTransformationUI(bool enable);
    }
}
