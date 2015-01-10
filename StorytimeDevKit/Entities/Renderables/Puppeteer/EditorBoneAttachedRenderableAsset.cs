using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Resources;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Entities.Renderables.Puppeteer
{
    public class EditorBoneAttachedRenderableAsset : BoneAttachedRenderableAsset
    {
        public AssetListItemViewModel AssetListItem { get; set; }
    }
}
