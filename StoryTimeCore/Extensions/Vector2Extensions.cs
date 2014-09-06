﻿using System;
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

        public static Vector2 Rotate(this Vector2 vec, float rotation)
        {
            return vec.Rotate(rotation, Vector2.Zero);
        }

        public static Vector2 Rotate(this Vector2 vec, float rotation, Vector2 center)
        {
            rotation = (rotation) * ((float)Math.PI / 180.0f); // Convert to radians
            float rotatedX = (float)(Math.Cos(rotation) * (vec.X - center.X) - Math.Sin(rotation) * (vec.Y - center.Y) + center.X);
            float rotatedY = (float)(Math.Sin(rotation) * (vec.X - center.X) + Math.Cos(rotation) * (vec.Y - center.Y) + center.Y);
            return new Vector2(rotatedX, rotatedY);
        }

        public static Vector2 GetScaled(
            this Vector2 vec, Vector2 scale, Vector2 center)
        {
            float xDisplacementToCenter = vec.X - center.X;
            float yDisplacementToCenter = vec.Y - center.Y;
            return new Vector2(vec.X + scale.X * xDisplacementToCenter, vec.Y + scale.Y * yDisplacementToCenter);
        }

        public static float AngleWithCenterIn(this Vector2 point, Vector2 center)
        {
            float angle = (float)Math.Atan2(point.Y - center.Y, point.X - center.X) * 180 / MathHelper.Pi;
            return angle;
        }
    }
}
