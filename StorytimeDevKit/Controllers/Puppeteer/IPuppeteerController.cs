using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeFramework.WorldManagement;
using StoryTimeDevKit.Models.SavedData.Bones;
using Puppeteer.Armature;
using StoryTimeDevKit.Models.Puppeteer;
using System.IO;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface IPuppeteerController : IStackedCommandsController<IPuppeteerEditorControl>
    {
        IPuppeteerEditorControl PuppeteerControl { get; set; }
        GameWorld GameWorld { get; }
        SavePuppeteerItemDialogModel SavedPuppeteerItemModel { get; }
        Skeleton Skeleton { get; }
        bool HasAnimations { get; }
        void SaveSkeleton();
        void SaveAnimatedSkeleton();
        void Load(FileInfo file);
    }
}
