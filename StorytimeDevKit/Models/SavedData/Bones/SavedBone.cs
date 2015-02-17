using StoryTimeDevKit.Models.SavedData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedBone
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlArray("Children")]
        public SavedBone[] Children { get; set; }
        [XmlElement("AbsolutePosition")]
        public SavedVector2 AbsolutePosition { get; set; }
        [XmlElement("AbsoluteEnd")]
        public SavedVector2 AbsoluteEnd { get; set; }
    }
}
