using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 Negative(this Vector2 vec)
        {
            return new Vector2(-vec.X, -vec.Y);
        }
    }
}
