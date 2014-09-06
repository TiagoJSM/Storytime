using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Extensions
{
    public static class RectanglefExtensions
    {
        public static AxisAlignedBoundingBox2D Combine(this AxisAlignedBoundingBox2D rec1, AxisAlignedBoundingBox2D rec2)
        {
            float combinedTop, combinedBottom, combinedLeft, combinedRight;

            combinedTop = rec1.Top.SmallerOrEqualThan(rec2.Top);
            combinedBottom = rec1.Bottom.BiggerOrEqualThan(rec2.Bottom);
            combinedLeft = rec1.Left.SmallerOrEqualThan(rec2.Left);
            combinedRight = rec1.Right.BiggerOrEqualThan(rec2.Right);

            return new AxisAlignedBoundingBox2D(
                combinedLeft, 
                combinedTop, 
                combinedBottom - combinedTop, 
                combinedRight - combinedLeft
            ); 
        }

        public static AxisAlignedBoundingBox2D GetRotated(
            this AxisAlignedBoundingBox2D rec, float rotation)
        {
            return rec.GetRotated(rotation, rec.Center);
        }

        public static AxisAlignedBoundingBox2D GetRotated(
            this AxisAlignedBoundingBox2D rec, float rotation, Vector2 rotationOrigin)
        {
            Vector2 rotatedBottomLeft = rec.BottomLeft.Rotate(rotation, rotationOrigin);
            Vector2 rotatedBottomRight = rec.BottomRight.Rotate(rotation, rotationOrigin);
            Vector2 rotatedTopLeft = rec.TopLeft.Rotate(rotation, rotationOrigin);
            Vector2 rotatedTopRight = rec.TopRight.Rotate(rotation, rotationOrigin);

            float[] xValues = new float[] { rotatedBottomLeft.X, rotatedBottomRight.X, rotatedTopLeft.X, rotatedTopRight.X };
            float[] yValues = new float[] { rotatedBottomLeft.Y, rotatedBottomRight.Y, rotatedTopLeft.Y, rotatedTopRight.Y };
            
            float smallestX = xValues.Min();
            float biggestX = xValues.Max();
            float smallestY = yValues.Min();
            float biggestY = yValues.Max();

            return new AxisAlignedBoundingBox2D(smallestX, smallestY, biggestX - smallestX, biggestY - smallestY);
        }

        public static AxisAlignedBoundingBox2D GetScaled(
            this AxisAlignedBoundingBox2D rec, Vector2 scale)
        {
            return rec.GetScaled(scale, rec.Center);
        }

        public static AxisAlignedBoundingBox2D GetScaled(
            this AxisAlignedBoundingBox2D rec, Vector2 scale, Vector2 scaleOrigin)
        {
            Vector2 scaledBottomLeft = rec.BottomLeft.GetScaled(scale, scaleOrigin);
            Vector2 scaledTopRight = rec.TopRight.GetScaled(scale, scaleOrigin);
            float height = scaledTopRight.Y - scaledBottomLeft.Y;
            float width = scaledTopRight.X - scaledBottomLeft.X;

            return new AxisAlignedBoundingBox2D(
                scaledBottomLeft.X, 
                scaledBottomLeft.Y,
                height,
                width);
        }
    }
}
