using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Input.Time;

namespace StoryTimeCore.Resources.Graphic
{
    public abstract class TemplateRenderableAsset : IRenderableAsset
    {
        public virtual bool IsVisible { get; set; }

        public abstract event Action<IRenderableAsset> OnBoundingBoxChanges;

        protected void Render(IRenderer renderer, ITexture2D texture, Rectanglef boundingBox, float rotation)
        {
            if (!IsVisible) return;

            renderer
                .Render(
                    texture,
                    boundingBox.X,
                    boundingBox.Y,
                    boundingBox.Width,
                    boundingBox.Height,
                    rotation
                );
        }

        public abstract void Render(IRenderer renderer);
        public virtual void TimeElapse(WorldTime WTime) { }
        public abstract Rectanglef BoundingBox { get; }
    }
}
