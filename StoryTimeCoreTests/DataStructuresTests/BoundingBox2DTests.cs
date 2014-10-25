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
            BoundingBox2D box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            Vector2 point = new Vector2(5.0f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }

        [TestMethod]
        public void PointIsOutsideBoundingBox()
        {
            BoundingBox2D box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            Vector2 point = new Vector2(15.0f, 5.0f);

            Assert.AreEqual(false, box.Contains(point));
        }

        [TestMethod]
        public void PointIsInBoundingBoxBorders()
        {
            BoundingBox2D box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            Vector2 point = new Vector2(0.0f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }

        [TestMethod]
        public void PointIsInRotatedBoundingBox()
        {
            BoundingBox2D box = new BoundingBox2D(
                Vector2.Zero,
                new Vector2(0.0f, 10.0f),
                new Vector2(10.0f, 10.0f),
                new Vector2(10.0f, 0.0f));

            box = box.GetRotated(45.0f);

            Vector2 point = new Vector2(0.5f, 5.0f);

            Assert.AreEqual(true, box.Contains(point));
        }
    }
}
