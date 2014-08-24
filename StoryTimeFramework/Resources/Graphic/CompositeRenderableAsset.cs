using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Extensions;

namespace StoryTimeCore.Resources.Graphic
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

        protected override Rectanglef RawBoundingBox
        {
            get 
            {
                if (_assets == null || _assets.Length == 0)
                    return new Rectanglef();

                Rectanglef box = _assets[0].BoundingBox;
                for (int idx = 1; idx < _assets.Length; idx++)
                    box = box.Combine(_assets[idx].BoundingBox);
                return box;     
            }
        }

        protected IRenderableAsset[] Assets { get { return _assets; } }
    }
}
