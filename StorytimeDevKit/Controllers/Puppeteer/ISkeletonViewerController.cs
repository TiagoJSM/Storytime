using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Controls.Puppeteer;
using Puppeteer.Resources;
using System.Collections.ObjectModel;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface ISkeletonViewerController : IController<ISkeletonTreeViewControl>
    {
        ISkeletonTreeViewControl SkeletonTreeViewControl { get; set; }
        IRenderableAssetOrderControl RenderableAssetOrderControl { get; set; }
        SkeletonViewModel SkeletonViewModel { get; }
        ObservableCollection<AssetViewModel> RenderableAssetOrderModels { get; }

        BoneViewModel GetBoneViewModelByName(string name);
        void SelectBone(BoneViewModel model);
    }
}
