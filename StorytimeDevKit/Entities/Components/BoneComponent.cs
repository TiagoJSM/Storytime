using Microsoft.Xna.Framework;
using StoryTimeFramework.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Entities.Components
{
    public class BoneComponent : Static2DComponent
    {
        public BoneComponent()
        {
            Texture2DName = "Bone";
            OnCreated += OnCreatedHandler;
        }

        private void OnCreatedHandler()
        {
            var texture = LoadTexture2D(Texture2DName);
            Origin = new Vector2(RawAABoundingBox.Width / 2, 0.0f);
            RenderingOffset = -new Vector2(RawAABoundingBox.Width / 2, 0.0f);
        }
    }
}
