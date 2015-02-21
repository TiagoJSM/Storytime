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
using Puppeteer.Armature;
using StoryTimeCore.General;

namespace Puppeteer.Entities
{
    public class SkeletonComponent : Component
    {
        private List<BoneAttachedRenderableAsset> _renderables;
        private AxisAlignedBoundingBox2D _box;

        public Skeleton Skeleton { get; private set; }

        public IEnumerable<BoneAttachedRenderableAsset> BoneAttachedAssets { get { return _renderables; } }

        public SkeletonComponent()
        {
            Skeleton = new Skeleton();
            _renderables = new List<BoneAttachedRenderableAsset>();
        }

        public override void TimeElapse(WorldTime WTime)
        {
            //ToDo: Problem here is that the bone doesn't warn the asset that he has changed,
            //So a check has to be done each cycle
            //probably the best is that the asset should make the check
            UpdateBounds();
        }

        public void Add(BoneAttachedRenderableAsset asset)
        {
            _renderables.Add(asset);
            //asset.OnBoundingBoxChanges += AssetOnBoundingBoxChangesHandler;
            UpdateBounds();
        }

        public void Move(BoneAttachedRenderableAsset asset, int index)
        {
            _renderables.Move(asset, index);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return _box; }
        }

        protected override void DoRender(IRenderer renderer)
        {
            foreach (IRenderableAsset asset in _renderables)
                asset.Render(renderer);
        }

        /*private void AssetOnBoundingBoxChangesHandler(IBoundingBoxable boundingBoxable)
        {
            UpdateBounds();
        }*/

        private void UpdateBounds()
        {
            var currentBox = _renderables.Select(r => r.AABoundingBox).Combine();
            if (currentBox.Equals(_box))
            {
                return;
            }
            _box = currentBox;
            RaiseBoundingBoxChanges();
        }
    }
}
