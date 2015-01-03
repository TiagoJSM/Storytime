using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Extensions
{
    public static class AxisAlignedBoundingBox2DExtensions
    {
        public static AxisAlignedBoundingBox2D Combine(this AxisAlignedBoundingBox2D rec1, AxisAlignedBoundingBox2D rec2)
        {
            float combinedTop, combinedBottom, combinedLeft, combinedRight;

            combinedTop = rec1.Top.BiggerOrEqualThan(rec2.Top);
            combinedBottom = rec1.Bottom.SmallerOrEqualThan(rec2.Bottom);
            combinedLeft = rec1.Left.SmallerOrEqualThan(rec2.Left);
            combinedRight = rec1.Right.BiggerOrEqualThan(rec2.Right);

            return new AxisAlignedBoundingBox2D(
                combinedLeft,
                combinedBottom,
                combinedTop - combinedBottom, 
                combinedRight - combinedLeft
            ); 
        }

        public static AxisAlignedBoundingBox2D Combine(this IEnumerable<AxisAlignedBoundingBox2D> boxes)
        {
            if (!boxes.Any()) return new AxisAlignedBoundingBox2D();
            
            AxisAlignedBoundingBox2D? result = null;

            foreach (var box in boxes)
            {
                if (result == null)
                {
                    result = box;
                    continue;
                }

                result = result.Value.Combine(box);
            }

            return result.Value;
        }

        public static AxisAlignedBoundingBox2D GetRotated(
            this AxisAlignedBoundingBox2D rec, float rotation)
        {
            return rec.GetRotated(rotation, rec.Center);
        }

        public static AxisAlignedBoundingBox2D GetRotated(
            this AxisAlignedBoundingBox2D rec, float rotation, Vector2 rotationOrigin)
        {
            var rotatedBottomLeft = rec.BottomLeft.Rotate(rotation, rotationOrigin);
            var rotatedBottomRight = rec.BottomRight.Rotate(rotation, rotationOrigin);
            var rotatedTopLeft = rec.TopLeft.Rotate(rotation, rotationOrigin);
            var rotatedTopRight = rec.TopRight.Rotate(rotation, rotationOrigin);

            var xValues = new float[] { rotatedBottomLeft.X, rotatedBottomRight.X, rotatedTopLeft.X, rotatedTopRight.X };
            var yValues = new float[] { rotatedBottomLeft.Y, rotatedBottomRight.Y, rotatedTopLeft.Y, rotatedTopRight.Y };
            
            var smallestX = xValues.Min();
            var biggestX = xValues.Max();
            var smallestY = yValues.Min();
            var biggestY = yValues.Max();

            return new AxisAlignedBoundingBox2D(smallestX, smallestY, biggestY - smallestY, biggestX - smallestX);
        }

        public static AxisAlignedBoundingBox2D GetScaled(
            this AxisAlignedBoundingBox2D rec, Vector2 scale)
        {
            return rec.GetScaled(scale, rec.Center);
        }

        public static AxisAlignedBoundingBox2D GetScaled(
            this AxisAlignedBoundingBox2D rec, Vector2 scale, Vector2 scaleOrigin)
        {
            var scaledBottomLeft = rec.BottomLeft.GetScaled(scale, scaleOrigin);
            var scaledTopRight = rec.TopRight.GetScaled(scale, scaleOrigin);
            var height = scaledTopRight.Y - scaledBottomLeft.Y;
            var width = scaledTopRight.X - scaledBottomLeft.X;

            return new AxisAlignedBoundingBox2D(
                scaledBottomLeft.X, 
                scaledBottomLeft.Y,
                height,
                width);
        }

        public static BoundingBox2D GetBoundingBox2D(
            this AxisAlignedBoundingBox2D rec)
        {
            return new BoundingBox2D(
                rec.BottomLeft,
                rec.TopLeft,
                rec.TopRight,
                rec.BottomRight);
        }
    }
}
