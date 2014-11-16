using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace TestHelpers
{
    public class AssetHelpers
    {
        public static void AreEqual(Vector2 expected, Vector2 actual, float delta)
        {
            AssetHelpers.AreEqual(expected, actual, new Vector2(delta));
        }

        public static void AreEqual(Vector2 expected, Vector2 actual, Vector2 delta)
        {
            Assert.AreEqual(expected.X, actual.X, delta.X);
            Assert.AreEqual(expected.Y, actual.Y, delta.Y);
        }
    }
}
