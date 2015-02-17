using Puppeteer.Resources;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Resources.Graphic;
using StoryTimeFramework.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Extensions;

namespace Puppeteer.Entities
{
    public class SkeletonComponent : Component
    {
        private List<BoneAttachedRenderableAsset> _renderables;
        private AxisAlignedBoundingBox2D _box;

        public IEnumerable<BoneAttachedRenderableAsset> BoneAttachedAssets { get { return _renderables; } }

        public SkeletonComponent()
        {
            _renderables = new List<BoneAttachedRenderableAsset>();
        }

        public override void TimeElapse(WorldTime WTime)
        {
            
        }

        public void Add(BoneAttachedRenderableAsset asset)
        {
            _renderables.Add(asset);
            _box = _renderables.Select(r => r.AABoundingBox).Combine();
            RaiseBoundingBoxChanges();
        }

        public void Move(BoneAttachedRenderableAsset asset, int index)
        {
            _renderables.Move(asset, index);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { throw new NotImplementedException(); }
        }

        protected override void DoRender(IRenderer renderer)
        {
            foreach (IRenderableAsset asset in _renderables)
                asset.Render(renderer);
        }
    }
}
