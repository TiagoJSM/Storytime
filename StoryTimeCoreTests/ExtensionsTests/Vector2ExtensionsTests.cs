using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryTimeCore.Extensions;
using Microsoft.Xna.Framework;

namespace StoryTimeCoreTests.ExtensionsTests
{
    [TestClass]
    public class Vector2ExtensionsTests
    {
        [TestMethod]
        public void AngleWithCenterInOriginAndPointInYAxis()
        {
            var vec = new Vector2(0.0f, 3.0f);
            var angle = vec.AngleWithCenterIn(Vector2.Zero);
            Assert.AreEqual(90.0f, angle);
        }

        [TestMethod]
        public void AngleWithCenterInOriginAndPointWithSameCoordinates()
        {
            var vec = new Vector2(3.0f);
            var angle = vec.AngleWithCenterIn(Vector2.Zero);
            Assert.AreEqual(45.0f, angle);
        }

        [TestMethod]
        public void AngleWithoutCenterInOriginAndPointWithSameCoordinates()
        {
            var vec = new Vector2(3.0f);
            var angle = vec.AngleWithCenterIn(new Vector2(1.0f));
            Assert.AreEqual(45.0f, angle);
        }
    }
}
