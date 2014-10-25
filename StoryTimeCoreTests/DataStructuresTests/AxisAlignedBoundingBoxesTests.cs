using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeCoreTests.DataStructuresTests
{
    [TestClass]
    public class AxisAlignedBoundingBoxesTests
    {
        [TestMethod]
        public void RectangleContainsAnotherSmallerOne()
        {
            AxisAlignedBoundingBox2D bigger = new AxisAlignedBoundingBox2D(0, 0, 5.0f);
            AxisAlignedBoundingBox2D smaller = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(bigger.Contains(smaller));
        }

        [TestMethod]
        public void RectangleContainsAnotherWithTheSameSize()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(0, 0, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(0, 0, 5.0f);

            Assert.IsTrue(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleDoesntContainerABiggerOneLocatedInside()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(0, 0, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 10.0f);

            Assert.IsFalse(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleDoesntContainerABiggerTotallyOutside()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(0, 0, 10.0f);

            Assert.IsFalse(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleContainsAPointAsAVertice()
        {
            AxisAlignedBoundingBox2D rec = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(1.0f, 1.0f);

            Assert.IsTrue(rec.Contains(point));
        }

        [TestMethod]
        public void RectangleContainsAPointSomewhereInside()
        {
            AxisAlignedBoundingBox2D rec = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(2.0f, 2.0f);

            Assert.IsTrue(rec.Contains(point));
        }

        [TestMethod]
        public void RectangleDoesntContainAPointSomewhereOutside()
        {
            AxisAlignedBoundingBox2D rec = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(0.0f, 0.0f);

            Assert.IsFalse(rec.Contains(point));
        }

        [TestMethod]
        public void BiggerRectangleIntersectsSmallerNonTouchingRectangle()
        {
            AxisAlignedBoundingBox2D bigger = new AxisAlignedBoundingBox2D(0.0f, 0.0f, 5.0f);
            AxisAlignedBoundingBox2D smaller = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(bigger.Intersects(smaller));
        }

        [TestMethod]
        public void SmallerRectangleIntersectsBiggerNonTouchingRectangle()
        {
            AxisAlignedBoundingBox2D bigger = new AxisAlignedBoundingBox2D(0.0f, 0.0f, 5.0f);
            AxisAlignedBoundingBox2D smaller = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(smaller.Intersects(bigger));
        }

        [TestMethod]
        public void RectangleIntersectsTouchingRectangle()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(2.0f, 2.0f, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(rec1.Intersects(rec2));
        }

        [TestMethod]
        public void RectanglesDontIntersectEachOther()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(10.0f, 10.0f, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);

            Assert.IsFalse(rec1.Intersects(rec2));
        }

        [TestMethod]
        public void RectangleCombinationIsTheSameAsBoth()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 2.0f);
            AxisAlignedBoundingBox2D combined = rec1.Combine(rec2);

            Assert.IsTrue(combined.Equals(rec1));
            Assert.IsTrue(combined.Equals(rec2));
        }

        [TestMethod]
        public void RectangleCombinationWithIntersectedRectangles()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(3.0f, 3.0f, 4.0f, 1.0f);
            AxisAlignedBoundingBox2D combined = rec1.Combine(rec2);

            Assert.AreEqual(7.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(1.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right); 
        }

        [TestMethod]
        public void RectangleCombinationWithNonIntersectedRectangles()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 3.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(5.0f, 6.0f, 4.0f, 1.0f);
            AxisAlignedBoundingBox2D combined = rec1.Combine(rec2);

            Assert.AreEqual(10.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(1.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right);
        }

        [TestMethod]
        public void RectangleCombinationWithContainedRectangle()
        {
            AxisAlignedBoundingBox2D rec1 = new AxisAlignedBoundingBox2D(1.0f, 1.0f, 5.0f);
            AxisAlignedBoundingBox2D rec2 = new AxisAlignedBoundingBox2D(2.0f, 2.0f, 1.0f, 1.0f);
            AxisAlignedBoundingBox2D combined = rec1.Combine(rec2);
            
            Assert.AreEqual(6.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(1.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right);
        }
    }
}
