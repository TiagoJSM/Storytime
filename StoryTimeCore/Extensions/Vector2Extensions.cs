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

        public static Vector2 Inverse(this Vector2 vec)
        {
            return new Vector2(1/vec.X, 1/vec.Y);
        }

        public static Vector2 Rotate(this Vector2 vec, float rotation)
        {
            return vec.Rotate(rotation, Vector2.Zero);
        }

        public static Vector2 Rotate(this Vector2 vec, float rotation, Vector2 center)
        {
            rotation = (rotation) * ((float)Math.PI / 180.0f); // Convert to radians
            var rotatedX = (float)(Math.Cos(rotation) * (vec.X - center.X) - Math.Sin(rotation) * (vec.Y - center.Y) + center.X);
            var rotatedY = (float)(Math.Sin(rotation) * (vec.X - center.X) + Math.Cos(rotation) * (vec.Y - center.Y) + center.Y);
            return new Vector2(rotatedX, rotatedY);
        }

        public static Vector2 GetScaled(
            this Vector2 vec, Vector2 scale, Vector2 center)
        {
            var xDisplacementToCenter = vec.X - center.X;
            var yDisplacementToCenter = vec.Y - center.Y;
            return new Vector2(scale.X * xDisplacementToCenter, scale.Y * yDisplacementToCenter);
        }

        public static float AngleWithCenterIn(this Vector2 point, Vector2 center)
        {
            var angle = (float)Math.Atan2(point.Y - center.Y, point.X - center.X) * 180 / MathHelper.Pi;
            if (angle < 0)
            {
                angle = angle + 360;
            }
            return angle;
        }

        public static Vector3 ToVector3(this Vector2 vec2)
        {
            return new Vector3(vec2.X, vec2.Y, 0);
        }
    }
}
