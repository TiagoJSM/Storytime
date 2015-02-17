using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedAnimations
    {
        [XmlArray("SavedAnimationCollection")]
        public SavedAnimation[] SavedAnimationCollection { get; set; }
    }
}
