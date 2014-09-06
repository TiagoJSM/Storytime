using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.DataStructures;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.General;

namespace StoryTimeFrameworkTests.DataStructuresTests
{
    [TestClass]
    public class QuadtreeTests
    {
        class BoundableDummy : IBoundingBoxable
        {
            private AxisAlignedBoundingBox2D _boundingBox;

            public BoundableDummy(AxisAlignedBoundingBox2D boundingBox)
            {
                _boundingBox = boundingBox;
            }

            public AxisAlignedBoundingBox2D BoundingBox
            {
                get { return _boundingBox; }
            }
        }

        [TestMethod]
        public void DataIsQueriableAsDesired()
        {
            Quadtree<BoundableDummy> qtree = new Quadtree<BoundableDummy>();
            BoundableDummy dummy = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 10.0f, 20.0f)
                );

            qtree.Add(dummy);
            AxisAlignedBoundingBox2D searchBox = new AxisAlignedBoundingBox2D(0, 0, 50.0f);
            int hitCount = 0;
            Action<BoundableDummy> hitAction = (BoundableDummy) => hitCount++;

            qtree.Query(searchBox, hitAction);
            Assert.AreEqual(1, hitCount);
        }

        [TestMethod]
        public void DataIsNotQueriableAsDesired()
        {
            Quadtree<BoundableDummy> qtree = new Quadtree<BoundableDummy>();
            BoundableDummy dummy = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 10.0f, 20.0f)
                );

            qtree.Add(dummy);
            AxisAlignedBoundingBox2D searchBox = new AxisAlignedBoundingBox2D(100.0f, 100.0f, 50.0f);
            int hitCount = 0;
            Action<BoundableDummy> hitAction = (BoundableDummy) => hitCount++;

            qtree.Query(searchBox, hitAction);
            Assert.AreEqual(0, hitCount);
        }

        [TestMethod]
        public void QueryQuadtreeWithTwoDummiesAndObtainOnlyOne()
        {
            Quadtree<BoundableDummy> qtree = new Quadtree<BoundableDummy>();
            BoundableDummy dummy1 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 10.0f, 20.0f)
                );
            BoundableDummy dummy2 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(100.0f, 100.0f, 20.0f)
                );

            qtree.Add(dummy1);
            qtree.Add(dummy2);
            AxisAlignedBoundingBox2D searchBox = new AxisAlignedBoundingBox2D(10.0f, 10.0f, 50.0f);
            List<BoundableDummy> hitDummies = new List<BoundableDummy>();
            Action<BoundableDummy> hitAction = (bd) => hitDummies.Add(bd);

            qtree.Query(searchBox, hitAction);
            Assert.AreEqual(1, hitDummies.Count);
            Assert.AreEqual(dummy1, hitDummies[0]);
        }

        [TestMethod]
        public void QueryQuadtreeWithTwoDummiesAndObtainNone()
        {
            Quadtree<BoundableDummy> qtree = new Quadtree<BoundableDummy>();
            BoundableDummy dummy1 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 10.0f, 20.0f)
                );
            BoundableDummy dummy2 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(100.0f, 100.0f, 20.0f)
                );

            qtree.Add(dummy1);
            qtree.Add(dummy2);
            AxisAlignedBoundingBox2D searchBox = new AxisAlignedBoundingBox2D(50.0f, 50.0f, 5.0f);
            List<BoundableDummy> hitDummies = new List<BoundableDummy>();
            Action<BoundableDummy> hitAction = (bd) => hitDummies.Add(bd);

            qtree.Query(searchBox, hitAction);
            Assert.AreEqual(0, hitDummies.Count);
        }

        [TestMethod]
        public void QueryQuadtreeWithThreeDummiesAndObtainAll()
        {
            Quadtree<BoundableDummy> qtree = new Quadtree<BoundableDummy>();
            BoundableDummy dummy1 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 10.0f, 20.0f)
                );
            BoundableDummy dummy2 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(100.0f, 100.0f, 20.0f)
                );
            BoundableDummy dummy3 = new BoundableDummy(
                new AxisAlignedBoundingBox2D(10.0f, 60.0f, 20.0f)
                );

            qtree.Add(dummy1);
            qtree.Add(dummy2);
            qtree.Add(dummy3);
            
            List<BoundableDummy> hitDummies = new List<BoundableDummy>();
            Action<BoundableDummy> hitAction = (bd) => hitDummies.Add(bd);

            qtree.Query(hitAction);
            Assert.AreEqual(3, hitDummies.Count);
        }
    }
}
