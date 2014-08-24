using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Input.Time;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeCore.Resources.Graphic
{
    public abstract class TemplateRenderableAsset : IRenderableAsset
    {
        private Vector2 _origin;
        public event Action<IRenderableAsset> OnBoundingBoxChanges;

        public virtual bool IsVisible { get; set; }

        public Vector2 Origin 
        {
            get
            {
                return _origin;
            }
            set
            {
                if (_origin != value)
                {
                    _origin = value;
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
                }
            }
        }

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
        public Rectanglef BoundingBox 
        {
            get
            {
                Rectanglef computedBox = RawBoundingBox;
                computedBox.Translate(Origin.Negative());
                return computedBox;
            }
        }

        protected void RaiseOnBoundingBoxChanges()
        {
            if(OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
        }

        protected abstract Rectanglef RawBoundingBox { get; }

    }
}
