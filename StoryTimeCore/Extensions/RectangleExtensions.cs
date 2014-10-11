using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle GetScaled(this Rectangle rec, Vector2 scale)
        {
            return new Rectangle(rec.X, rec.Y, rec.Width * (int)scale.X, rec.Height * (int)scale.Y);
        }
    }
}
