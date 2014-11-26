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
        private Vector2 _difference;

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
                AxisAlignedBoundingBox2D bb = new AxisAlignedBoundingBox2D(0, 0, _texture.Height, _texture.Width);
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
                _difference = RenderingOffset - Bone.AbsolutePosition;
                RenderingOffset = Vector2.Zero;// RenderingOffset - Bone.AbsolutePosition;
            }
        }

        public override void Render(IRenderer renderer)
        {
            Vector2 boneTranslation = Vector2.Zero;
            if (Bone != null)
            {
                Vector2 position;
                float rotation;
                Vector2 scale;

                Matrix transformation = 
                    MatrixUtils.CreateLocalTransformation(_difference, Rotation, Scale) * 
                    Bone.Transformation;

                transformation.DecomposeMatrix(out position, out rotation, out scale);
                boneTranslation = position;
                Rotation = rotation;
                Scale = scale;
            }
            
            //Rotation = Bone.Rotation;
            //should take into account original values before attaching a bone
            //Origin = AABoundingBox.BottomLeft - Bone.RelativePosition;
            AxisAlignedBoundingBox2D boundings = RawAABoundingBox;
            boundings.Translate(boneTranslation);
            Render(renderer, _texture, boundings);
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox { get { return _box; } }
    }
}
