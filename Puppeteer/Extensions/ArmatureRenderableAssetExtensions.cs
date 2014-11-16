using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Resources;
using Microsoft.Xna.Framework;

namespace Puppeteer.Extensions
{
    public static class ArmatureRenderableAssetExtensions
    {
        public static IEnumerable<BoneAttachedRenderableAsset> GetIntersectedBoneAttachedAssets(
            this ArmatureRenderableAsset armature, Vector2 point)
        {
            List<BoneAttachedRenderableAsset> assets = new List<BoneAttachedRenderableAsset>();

            foreach (BoneAttachedRenderableAsset asset in armature.BoneAttachedAssets)
                if (asset.BoundingBox.Contains(point))
                    assets.Add(asset);

            return assets;
        }
    }
}
