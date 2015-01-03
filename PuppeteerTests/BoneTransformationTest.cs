using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using TestHelpers;

namespace PuppeteerTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BoneTransformationTest
    {
        [TestMethod]
        public void RelativeEndIsConsistent()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(100);
            boneRoot.Length = 128;

            AssetHelpers.AreEqual(new Vector2(100, 228), boneRoot.AbsoluteEnd, 0.00001f);

            boneChild.AbsolutePosition = boneRoot.AbsoluteEnd;
            boneChild.AbsoluteEnd = new Vector2(250, 378);

            AssetHelpers.AreEqual(new Vector2(250, 378), boneChild.AbsoluteEnd, 0.00001f);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(400, 528);

            AssetHelpers.AreEqual(new Vector2(400, 528), leafChild.AbsoluteEnd, 0.00001f);
        }

        [TestMethod]
        public void MovingRootKeepsAllChildrenConsistent()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(100);
            boneRoot.Length = 128;

            boneChild.AbsolutePosition = boneRoot.AbsoluteEnd;
            boneChild.AbsoluteEnd = new Vector2(250, 378);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(400, 528);

            boneRoot.AbsolutePosition = new Vector2(150);

            AssetHelpers.AreEqual(new Vector2(150, 278), boneRoot.AbsoluteEnd, 0.00001f);
            AssetHelpers.AreEqual(new Vector2(300, 428), boneChild.AbsoluteEnd, 0.00001f);
            AssetHelpers.AreEqual(new Vector2(450, 578), leafChild.AbsoluteEnd, 0.00001f);
        }

        [TestMethod]
        public void RotatingRootKeepsAllChildrenConsistent()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.AbsolutePosition = boneRoot.AbsoluteEnd;
            boneChild.AbsoluteEnd = new Vector2(0, 200);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(0, 400);

            boneRoot.AbsoluteEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(200, 0), boneChild.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(400, 0), leafChild.AbsoluteEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructure()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.AbsolutePosition = boneRoot.AbsoluteEnd;
            boneChild.AbsoluteEnd = new Vector2(100, 0);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(200, 0);

            boneRoot.AbsoluteEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(0, -100), boneChild.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(0, -200), leafChild.AbsoluteEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructureWithTranslatedChild()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.AbsolutePosition = new Vector2(100);
            boneChild.AbsoluteEnd = new Vector2(200, 100);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(300, 100);

            boneRoot.AbsoluteEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -200), boneChild.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -300), leafChild.AbsoluteEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructureWithTranslatedChildAndRotateBack()
        {
            var boneRoot = new Bone();
            var boneChild = new Bone(boneRoot);
            var leafChild = new Bone(boneChild);

            boneRoot.AbsolutePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.AbsolutePosition = new Vector2(100);
            boneChild.AbsoluteEnd = new Vector2(200, 100);

            leafChild.AbsolutePosition = boneChild.AbsoluteEnd;
            leafChild.AbsoluteEnd = new Vector2(300, 100);

            boneRoot.AbsoluteEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -200), boneChild.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -300), leafChild.AbsoluteEnd, 0.0001f);

            boneRoot.AbsoluteEnd = new Vector2(0, 100);

            AssetHelpers.AreEqual(new Vector2(0, 100), boneRoot.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(200, 100), boneChild.AbsoluteEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(300, 100), leafChild.AbsoluteEnd, 0.0001f);
        }
    }
}
