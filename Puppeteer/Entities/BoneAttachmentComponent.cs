using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Extensions;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using StoryTimeFramework.Entities;

namespace Puppeteer.Entities
{
    public class BoneAttachmentComponent : Component
    {
        private Bone _bone;
        private Matrix _assetDefinedTransformation;
        private Matrix _boneReferenceTransformation;

        public ITexture2D Texture { get; set; }
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
                _assetDefinedTransformation = base.Transformation;
                _boneReferenceTransformation = Bone.Transformation;
                Rotation = 0;
                RenderingOffset = Vector2.Zero;
            }
        }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return Texture.GetAABoundingBox(); }
        }

        public BoneAttachmentComponent()
        {
            OnBoundingBoxChanges += OnBoundingBoxChangesHandler;
        }

        protected override void DoRender(IRenderer renderer)
        {
            RenderTexture(renderer, Texture);
        }

        public override void TimeElapse(WorldTime WTime)
        {
            RenderingOffset = Bone.AbsolutePosition;
            Rotation = Bone.Rotation;
        }

        private void OnBoundingBoxChangesHandler(WorldEntity entity)
        {
            _assetDefinedTransformation = base.Transformation;
        }
    }
}
