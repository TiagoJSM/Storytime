using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    [XmlRoot]
    public class SavedAnimatedSkeleton
    {
        [XmlElement("SavedSkeleton")]
        public SavedSkeleton SavedSkeleton { get; set; }
        [XmlElement("SavedAnimations")]
        public SavedAnimations SavedAnimations { get; set; }
    }
}
