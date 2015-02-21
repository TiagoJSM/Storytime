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

namespace Puppeteer.Entities
{
    public class BoneAttachmentComponent : Component
    {
        public ITexture2D Texture { get; set; }
        public Bone Bone { get; set; }

        protected override AxisAlignedBoundingBox2D RawAABoundingBox
        {
            get { return Texture.GetAABoundingBox(); }
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
    }
}
