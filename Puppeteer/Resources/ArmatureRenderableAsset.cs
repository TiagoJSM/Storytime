using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Extensions;

namespace Puppeteer.Resources
{
    public class ArmatureRenderableAsset : TemplateRenderableAsset
    {
        private List<BoneAttachedRenderableAsset> _renderables;
        private AxisAlignedBoundingBox2D _box;

        public IEnumerable<BoneAttachedRenderableAsset> BoneAttachedAssets { get { return _renderables; } }

        public ArmatureRenderableAsset()
        {
            _renderables = new List<BoneAttachedRenderableAsset>();
        }

        public override void Render(IRenderer renderer)
        {
            foreach (IRenderableAsset asset in _renderables)
                asset.Render(renderer);
        }

        public void Add(BoneAttachedRenderableAsset asset)
        {
            _renderables.Add(asset);
            _box = _renderables.Select(r => r.AABoundingBox).Combine();
            RaiseOnBoundingBoxChanges();
        }

        protected override AxisAlignedBoundingBox2D RawBoundingBox
        {
            get 
            {
                return _box; 
            }
        }
    }
}
