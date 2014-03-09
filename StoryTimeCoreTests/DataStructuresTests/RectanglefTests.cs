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
    public class RectanglefTests
    {
        [TestMethod]
        public void RectangleContainsAnotherSmallerOne()
        {
            Rectanglef bigger = new Rectanglef(0, 0, 5.0f);
            Rectanglef smaller = new Rectanglef(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(bigger.Contains(smaller));
        }

        [TestMethod]
        public void RectangleContainsAnotherWithTheSameSize()
        {
            Rectanglef rec1 = new Rectanglef(0, 0, 5.0f);
            Rectanglef rec2 = new Rectanglef(0, 0, 5.0f);

            Assert.IsTrue(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleDoesntContainerABiggerOneLocatedInside()
        {
            Rectanglef rec1 = new Rectanglef(0, 0, 5.0f);
            Rectanglef rec2 = new Rectanglef(1.0f, 1.0f, 10.0f);

            Assert.IsFalse(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleDoesntContainerABiggerTotallyOutside()
        {
            Rectanglef rec1 = new Rectanglef(1.0f, 1.0f, 5.0f);
            Rectanglef rec2 = new Rectanglef(0, 0, 10.0f);

            Assert.IsFalse(rec1.Contains(rec2));
        }

        [TestMethod]
        public void RectangleContainsAPointAsAVertice()
        {
            Rectanglef rec = new Rectanglef(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(1.0f, 1.0f);

            Assert.IsTrue(rec.Contains(point));
        }

        [TestMethod]
        public void RectangleContainsAPointSomewhereInside()
        {
            Rectanglef rec = new Rectanglef(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(2.0f, 2.0f);

            Assert.IsTrue(rec.Contains(point));
        }

        [TestMethod]
        public void RectangleDoesntContainAPointSomewhereOutside()
        {
            Rectanglef rec = new Rectanglef(1.0f, 1.0f, 5.0f);
            Vector2 point = new Vector2(0.0f, 0.0f);

            Assert.IsFalse(rec.Contains(point));
        }

        [TestMethod]
        public void BiggerRectangleIntersectsSmallerNonTouchingRectangle()
        {
            Rectanglef bigger = new Rectanglef(0.0f, 0.0f, 5.0f);
            Rectanglef smaller = new Rectanglef(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(bigger.Intersects(smaller));
        }

        [TestMethod]
        public void SmallerRectangleIntersectsBiggerNonTouchingRectangle()
        {
            Rectanglef bigger = new Rectanglef(0.0f, 0.0f, 5.0f);
            Rectanglef smaller = new Rectanglef(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(smaller.Intersects(bigger));
        }

        [TestMethod]
        public void RectangleIntersectsTouchingRectangle()
        {
            Rectanglef rec1 = new Rectanglef(2.0f, 2.0f, 5.0f);
            Rectanglef rec2 = new Rectanglef(1.0f, 1.0f, 2.0f);

            Assert.IsTrue(rec1.Intersects(rec2));
        }

        [TestMethod]
        public void RectanglesDontIntersectEachOther()
        {
            Rectanglef rec1 = new Rectanglef(10.0f, 10.0f, 5.0f);
            Rectanglef rec2 = new Rectanglef(1.0f, 1.0f, 2.0f);

            Assert.IsFalse(rec1.Intersects(rec2));
        }

        [TestMethod]
        public void RectangleCombinationIsTheSameAsBoth()
        {
            Rectanglef rec1 = new Rectanglef(1.0f, 1.0f, 2.0f);
            Rectanglef rec2 = new Rectanglef(1.0f, 1.0f, 2.0f);
            Rectanglef combined = rec1.Combine(rec2);

            Assert.IsTrue(combined.Equals(rec1));
            Assert.IsTrue(combined.Equals(rec2));
        }

        [TestMethod]
        public void RectangleCombinationWithIntersectedRectangles()
        {
            Rectanglef rec1 = new Rectanglef(1.0f, 1.0f, 5.0f);
            Rectanglef rec2 = new Rectanglef(3.0f, 3.0f, 4.0f, 1.0f);
            Rectanglef combined = rec1.Combine(rec2);

            Assert.AreEqual(1.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(7.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right); 
        }

        [TestMethod]
        public void RectangleCombinationWithNonIntersectedRectangles()
        {
            Rectanglef rec1 = new Rectanglef(1.0f, 1.0f, 3.0f);
            Rectanglef rec2 = new Rectanglef(5.0f, 6.0f, 4.0f, 1.0f);
            Rectanglef combined = rec1.Combine(rec2);

            Assert.AreEqual(1.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(10.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right);
        }

        [TestMethod]
        public void RectangleCombinationWithContainedRectangle()
        {
            Rectanglef rec1 = new Rectanglef(1.0f, 1.0f, 5.0f);
            Rectanglef rec2 = new Rectanglef(2.0f, 2.0f, 1.0f, 1.0f);
            Rectanglef combined = rec1.Combine(rec2);
            
            Assert.AreEqual(1.0f, combined.Top);
            Assert.AreEqual(1.0f, combined.Left);
            Assert.AreEqual(6.0f, combined.Bottom);
            Assert.AreEqual(6.0f, combined.Right);
        }
    }
}
