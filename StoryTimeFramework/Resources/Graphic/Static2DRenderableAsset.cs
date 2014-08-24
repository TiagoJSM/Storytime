﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.DataStructures;
using StoryTimeCore.Input.Time;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeFramework.Resources.Graphic
{
    public class Static2DRenderableAsset : TemplateRenderableAsset//IRenderableAsset
    {
        private Rectanglef _boundingBox;
        private ITexture2D _texture2D;

        public float Rotation { get; set; }

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
                    RaiseOnBoundingBoxChanges();
                }
            }
        }

        public Static2DRenderableAsset()
        {
            IsVisible = true;
        }
        
        public override void Render(IRenderer renderer)
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

        protected override Rectanglef RawBoundingBox
        {
            get { return _boundingBox; }
        }
    }
}
