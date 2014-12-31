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
    public class BoundingBox2DTests
    {
        [TestMethod]
        public void PointIsInsideBoundingBox()
        {
            var box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            var point = new Vector2(5.0f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }

        [TestMethod]
        public void PointIsOutsideBoundingBox()
        {
            var box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            var point = new Vector2(15.0f, 5.0f);

            Assert.AreEqual(false, box.Contains(point));
        }

        [TestMethod]
        public void PointIsInBoundingBoxBorders()
        {
            var box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            var point = new Vector2(0.0f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }

        [TestMethod]
        public void PointIsInRotatedBoundingBox()
        {
            var box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            box = box.GetRotated(45.0f);

            var point = new Vector2(0.5f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }
    }
}
