using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Extensions;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeFramework.Resources.Graphic
{
    public class CompositeRenderableAsset : TemplateRenderableAsset
    {
        private IRenderableAsset[] _assets;

        public CompositeRenderableAsset(params IRenderableAsset[] assets)
        {
            _assets = assets;
        }

        public override void Render(IRenderer renderer)
        {
            foreach (IRenderableAsset asset in _assets)
                asset.Render(renderer);
        }

        protected override AxisAlignedBoundingBox2D RawBoundingBox
        {
            get 
            {
                if (_assets == null || _assets.Length == 0)
                    return new AxisAlignedBoundingBox2D();

                AxisAlignedBoundingBox2D box = _assets[0].AABoundingBox;
                for (int idx = 1; idx < _assets.Length; idx++)
                    box = box.Combine(_assets[idx].AABoundingBox);
                return box;     
            }
        }

        protected IRenderableAsset[] Assets { get { return _assets; } }
    }
}
