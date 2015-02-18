using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Resources;
using Microsoft.Xna.Framework;
using Puppeteer.Entities;

namespace Puppeteer.Extensions
{
    public static class ArmatureExtensions
    {
        public static IEnumerable<BoneAttachedRenderableAsset> GetIntersectedBoneAttachedAssets(
            this ArmatureRenderableAsset armature, Vector2 point)
        {
            var assets = new List<BoneAttachedRenderableAsset>();

            foreach (var asset in armature.BoneAttachedAssets)
                if (asset.BoundingBox.Contains(point))
                    assets.Add(asset);

            return assets;
        }

        public static IEnumerable<BoneAttachedRenderableAsset> GetIntersectedBoneAttachedAssets(
            this SkeletonComponent component, Vector2 point)
        {
            var assets = new List<BoneAttachedRenderableAsset>();

            foreach (var asset in component.BoneAttachedAssets)
                if (asset.BoundingBox.Contains(point))
                    assets.Add(asset);

            return assets;
        }
    }
}
