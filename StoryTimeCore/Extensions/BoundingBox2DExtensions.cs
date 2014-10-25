using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;

namespace StoryTimeCore.Extensions
{
    public static class BoundingBox2DExtensions
    {
        public static BoundingBox2D GetRotated(
            this BoundingBox2D rec, float rotation)
        {
            return rec.GetRotated(rotation, rec.Center);
        }

        public static BoundingBox2D GetRotated(
            this BoundingBox2D rec, float rotation, Vector2 rotationOrigin)
        {
            Vector2 rotatedPoint1 = rec.Point1.Rotate(rotation, rotationOrigin);
            Vector2 rotatedPoint2 = rec.Point2.Rotate(rotation, rotationOrigin);
            Vector2 rotatedPoint3 = rec.Point3.Rotate(rotation, rotationOrigin);
            Vector2 rotatedPoint4 = rec.Point4.Rotate(rotation, rotationOrigin);

            return new BoundingBox2D(rotatedPoint1, rotatedPoint2, rotatedPoint3, rotatedPoint4);
        }

        public static BoundingBox2D GetScaled(
            this BoundingBox2D rec, Vector2 scale)
        {
            return rec.GetScaled(scale, rec.Center);
        }

        public static BoundingBox2D GetScaled(
            this BoundingBox2D rec, Vector2 scale, Vector2 scaleOrigin)
        {
            Vector2 scaledPoint1 = rec.Point1.GetScaled(scale, scaleOrigin);
            Vector2 scaledPoint2 = rec.Point2.GetScaled(scale, scaleOrigin);
            Vector2 scaledPoint3 = rec.Point3.GetScaled(scale, scaleOrigin);
            Vector2 scaledPoint4 = rec.Point4.GetScaled(scale, scaleOrigin);

            return new BoundingBox2D(
                scaledPoint1,
                scaledPoint2,
                scaledPoint3,
                scaledPoint4);
        }
    }
}
