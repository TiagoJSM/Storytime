using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    [XmlRoot("Skeleton")]
    public class SavedSkeleton
    {
        [XmlArray("RootBones")]
        public SavedBone[] RootBones { get; set; }
    }
}
