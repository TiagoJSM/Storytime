using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedBoneAnimation
    {
        [XmlElement("AnimationName")]
        public string BoneName { get; set; }
        [XmlArray("AnimationFrames")]
        public SavedAnimationFrame[] AnimationFrames { get; set; }
    }
}
