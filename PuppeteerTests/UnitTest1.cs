using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;

namespace PuppeteerTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BoneTransformationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Bone boneRoot = new Bone();
            Bone boneChild = new Bone(boneRoot);
            /*Bone leafChild = new Bone(boneChild);*/
            boneRoot.Length = 10.0f;
            boneChild.Length = 10.0f;
            boneRoot.RelativePosition = new Vector2(5);
            /*leafChild.Length = 10.0f;*/

            //boneRoot.Rotation = 90.0f;
            //boneChild.RelativeEnd = new Vector2(5, 30);
            //boneChild.Translation = new Vector2(5);
            boneRoot.Rotation = 90.0f;
            boneChild.Rotation = 90.0f;
            
            //Vector2 childPosition = boneChild.RelativePosition;
            //Vector2 leafPosition = leafChild.RelativePosition;
            //var v = boneChild.Transformation;
        }
    }
}
