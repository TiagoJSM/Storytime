using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedAnimation
    {
        [XmlElement("AnimationName")]
        public string AnimationName { get; set; }
        [XmlArray("BoneAnimationFrames")]
        public SavedBoneAnimation[] BoneAnimationFrames { get; set; }
    }
}
