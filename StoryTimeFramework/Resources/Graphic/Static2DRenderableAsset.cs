using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.DataStructures;
using StoryTimeCore.Input.Time;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;

namespace StoryTimeFramework.Resources.Graphic
{
    public class Static2DRenderableAsset : IRenderableAsset
    {
        private Rectanglef _boundingBox;
        private ITexture2D _texture2D;

        public ITexture2D Texture2D 
        {
            get
            {
                return _texture2D;
            }
            set
            {
                _texture2D = value;
                Rectanglef bb = new Rectanglef(0, 0, value.Height, value.Width);
                if (!_boundingBox.Equals(bb))
                {
                    _boundingBox = bb;
                    if (OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
                }
            }
        }
        public Rectanglef BoundingBox { get { return _boundingBox; } }
        public bool IsVisible { get; set; }
        public float Rotation { get; set; }

        public event Action<IRenderableAsset> OnBoundingBoxChanges;

        public Static2DRenderableAsset()
        {
            IsVisible = true;
        }

        public void SetBoundingBox(Rectanglef boundingBox)
        {
            _boundingBox = boundingBox;
            if(OnBoundingBoxChanges != null) OnBoundingBoxChanges(this);
        }

        public void Render(IRenderer renderer)
        {
            if (!IsVisible) return;

            renderer
                .Render(
                    Texture2D, 
                    BoundingBox.X, 
                    BoundingBox.Y, 
                    BoundingBox.Width, 
                    BoundingBox.Height, 
                    Rotation
                );
        }

        public void TimeElapse(WorldTime WTime) { }
    }
}
