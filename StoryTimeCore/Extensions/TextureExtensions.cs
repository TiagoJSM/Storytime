using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Extensions
{
    public static class TextureExtensions
    {
        public static AxisAlignedBoundingBox2D GetAABoundingBox(this ITexture2D texture)
        {
            return new AxisAlignedBoundingBox2D(0, 0, texture.Height, texture.Width);
        }
    }
}
