using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector2 ToVector2(this Vector3 vec3)
        {
            return new Vector2(vec3.X, vec3.Y);
        }
    }
}
