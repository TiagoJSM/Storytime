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
        [XmlElement("Length")]
        public float Length { get; set; }
        [XmlArray("Children")]
        public SavedBone[] Children { get; set; }
        [XmlElement("Translation")]
        public SavedVector2 Translation { get; set; }
        [XmlElement("Rotation")]
        public float Rotation { get; set; }
    }
}
