using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.DataStructures
{
    /// <summary>
    /// The struct that defines a rectangle built with float variables.
    /// It's a shallow implementation of XNA rectangle, but with float variables.
    /// </summary>
    public struct AxisAlignedBoundingBox2D
    {
        public float Height;
        public float Width;
        public float X;
        public float Y;

        public AxisAlignedBoundingBox2D(float x, float y, float sideSize)
            : this(x, y, sideSize, sideSize)
        {
        }

        public AxisAlignedBoundingBox2D(float x, float y, float height, float width)
            :this()
        {
            Height = height;
            Width = width;
            X = x;
            Y = y;
        }

        public AxisAlignedBoundingBox2D(Vector2 position)
            : this(position.X, position.Y, 0)
        {
        }

        public float Top { get { return Y; } }
        public float Bottom { get { return Y + Height; } }
        public float Left { get { return X; } }
        public float Right { get { return X + Width; } }
        public Vector2 Center
        {
            get
            {
                float CenterX = X + Width / 2;
                float CenterY = Y + Height / 2;
                return new Vector2(CenterX, CenterY);
            }
        }
        public Vector2 TopLeft { get { return new Vector2(Left, Top); } }
        public Vector2 TopRight { get { return new Vector2(Right, Top); } }
        public Vector2 BottomLeft { get { return new Vector2(Left, Bottom); } }
        public Vector2 BottomRight { get { return new Vector2(Right, Bottom); } }


        public void Translate(Vector2 translation)
        {
            X += translation.X;
            Y += translation.Y;
        }

        public bool Contains(Vector2 point)
        {
            if (!(X <= point.X && point.X <= Right))
                return false;
            if (!(Y <= point.Y && point.Y <= Bottom))
                return false;
            return true;
        }

        public bool Contains(AxisAlignedBoundingBox2D rec)
        {
            Vector2 TopLeftCorner = new Vector2(rec.Left, rec.Top);
            if (!Contains(TopLeftCorner))
                return false;

            Vector2 BottomRightCorner = new Vector2(rec.Right, rec.Bottom);
            if (!Contains(BottomRightCorner))
                return false;
             
            return true;
        }

        public bool Intersects(AxisAlignedBoundingBox2D rec)
        {
            if (ContainsAnyVerticesFrom(rec)) return true;
            if (rec.Contains(this)) return true;
            return false;
        }

        public bool ContainsAnyVerticesFrom(AxisAlignedBoundingBox2D rec)
        {
            if (Contains(rec.TopLeft)) return true;
            if (Contains(rec.TopRight)) return true;
            if (Contains(rec.BottomLeft)) return true;
            if (Contains(rec.BottomRight)) return true;
            return false;
        }

        public bool Equals(AxisAlignedBoundingBox2D rec)
        {
            if (Top != rec.Top) return false;
            if (Bottom != rec.Bottom) return false;
            if (Left != rec.Left) return false;
            if (Right != rec.Right) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            bool sameType = obj is AxisAlignedBoundingBox2D;
            if (!sameType)
                return false;
            return Equals((AxisAlignedBoundingBox2D)obj);
        }
    }
}
