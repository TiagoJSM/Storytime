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
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(100);
            boneRoot.Length = 128;

            AssetHelpers.AreEqual(new Vector2(100, 228), boneRoot.RelativeEnd, 0.00001f);

            boneChild.RelativePosition = boneRoot.RelativeEnd;
            boneChild.RelativeEnd = new Vector2(250, 378);

            AssetHelpers.AreEqual(new Vector2(250, 378), boneChild.RelativeEnd, 0.00001f);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(400, 528);

            AssetHelpers.AreEqual(new Vector2(400, 528), leafChild.RelativeEnd, 0.00001f);
        }

        [TestMethod]
        public void MovingRootKeepsAllChildrenConsistent()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(100);
            boneRoot.Length = 128;

            boneChild.RelativePosition = boneRoot.RelativeEnd;
            boneChild.RelativeEnd = new Vector2(250, 378);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(400, 528);

            boneRoot.RelativePosition = new Vector2(150);

            AssetHelpers.AreEqual(new Vector2(150, 278), boneRoot.RelativeEnd, 0.00001f);
            AssetHelpers.AreEqual(new Vector2(300, 428), boneChild.RelativeEnd, 0.00001f);
            AssetHelpers.AreEqual(new Vector2(450, 578), leafChild.RelativeEnd, 0.00001f);
        }

        [TestMethod]
        public void RotatingRootKeepsAllChildrenConsistent()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.RelativePosition = boneRoot.RelativeEnd;
            boneChild.RelativeEnd = new Vector2(0, 200);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(0, 400);

            boneRoot.RelativeEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(200, 0), boneChild.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(400, 0), leafChild.RelativeEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructure()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.RelativePosition = boneRoot.RelativeEnd;
            boneChild.RelativeEnd = new Vector2(100, 0);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(200, 0);

            boneRoot.RelativeEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(0, -100), boneChild.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(0, -200), leafChild.RelativeEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructureWithTranslatedChild()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.RelativePosition = new Vector2(100);
            boneChild.RelativeEnd = new Vector2(200, 100);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(300, 100);

            boneRoot.RelativeEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -200), boneChild.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -300), leafChild.RelativeEnd, 0.0001f);
        }

        [TestMethod]
        public void RotationsInBoneStructureWithTranslatedChildAndRotateBack()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            Bone leafChild = new Bone(boneChild);

            boneRoot.RelativePosition = new Vector2(0);
            boneRoot.Length = 100;

            boneChild.RelativePosition = new Vector2(100);
            boneChild.RelativeEnd = new Vector2(200, 100);

            leafChild.RelativePosition = boneChild.RelativeEnd;
            leafChild.RelativeEnd = new Vector2(300, 100);

            boneRoot.RelativeEnd = new Vector2(100, 0);

            AssetHelpers.AreEqual(new Vector2(100, 0), boneRoot.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -200), boneChild.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(100, -300), leafChild.RelativeEnd, 0.0001f);

            boneRoot.RelativeEnd = new Vector2(0, 100);

            AssetHelpers.AreEqual(new Vector2(0, 100), boneRoot.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(200, 100), boneChild.RelativeEnd, 0.0001f);
            AssetHelpers.AreEqual(new Vector2(300, 100), leafChild.RelativeEnd, 0.0001f);
        }
    }
}
