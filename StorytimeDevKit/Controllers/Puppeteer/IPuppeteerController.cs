using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeFramework.WorldManagement;
using StoryTimeDevKit.Models.SavedData.Bones;
using Puppeteer.Armature;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface IPuppeteerController : IStackedCommandsController<IPuppeteerEditorControl>
    {
        IPuppeteerEditorControl PuppeteerControl { get; set; }
        GameWorld GameWorld { get; }
        SaveSkeletonDialogModel SavedSkeletonModel { get; }
        Skeleton Skeleton { get; }
        void SaveSkeleton();
    }
}
