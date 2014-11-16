using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using Puppeteer.Armature;

namespace Puppeteer.Resources
{
    public class BoneAttachedRenderableAsset : TemplateRenderableAsset
    {
        private AxisAlignedBoundingBox2D _box;
        private ITexture2D _texture;
        private Bone _bone;

        public ITexture2D Texture 
        {
            get
            {
                return _texture;
            }
            set
            {
                if (_texture == value) return;
                _texture = value;
                _box = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
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
            }
        }

        public override void Render(IRenderer renderer)
        {
            //Rotation = Bone.Rotation;
            //should take into account original values before attaching a bone
            //Origin = AABoundingBox.BottomLeft - Bone.RelativePosition;
            Render(renderer, _texture, AABoundingBox);
        }

        protected override AxisAlignedBoundingBox2D RawBoundingBox { get { return _box; } }
    }
}
