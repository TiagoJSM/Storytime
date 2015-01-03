using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using StoryTimeCore.Utils;
using StoryTimeCore.Extensions;

namespace Puppeteer.Resources
{
    public class BoneAttachedRenderableAsset : TemplateRenderableAsset
    {
        private AxisAlignedBoundingBox2D _box;
        private ITexture2D _texture;
        private Bone _bone;
        private Matrix _assetDefinedTransformation;
        private Matrix _boneReferenceTransformation;

        public ITexture2D Texture2D
        {
            get
            {
                return _texture;
            }
            set
            {
                if (_texture == value) return;
                _texture = value;
                var bb = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
                if (!_box.Equals(bb))
                {
                    _box = bb;
                    RaiseOnBoundingBoxChanges();
                }
            }
        }
        public Bone Bone 
        {
            get 
            { 
                return _bone; 
            }
            set
            {
                if (_bone == value) return;
                _bone = value;
                var totalTransformation = base.Transformation * Bone.Transformation;
                _assetDefinedTransformation = totalTransformation * Matrix.Invert(Bone.Transformation);
                _boneReferenceTransformation = Bone.Transformation;
                Rotation = 0;
                Scale = new Vector2(1);
                RenderingOffset = Vector2.Zero;
            }
        }

        public override void Render(IRenderer renderer)
        {
            var boundings = RawAABoundingBox;
            Render(renderer, _texture, boundings);
        }

        protected override Matrix Transformation
        {
            get
            {
                if (Bone != null)
                {
                    var extra = Matrix.Invert(_boneReferenceTransformation) * Bone.Transformation;
                    return _assetDefinedTransformation * extra * base.Transformation;
                }
                return base.Transformation;
            }
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox { get { return _box; } }
    }
}
