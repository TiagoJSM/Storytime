using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedAnimationFrame
    {
        [XmlElement("StartTime")]
        public TimeSpan StartTime { get; set; }
        [XmlElement("EndTime")]
        public TimeSpan EndTime { get; set; }
        [XmlElement("StartTranslation")]
        public Vector2 StartTranslation { get; set; }
        [XmlElement("StartRotation")]
        public float StartRotation { get; set; }
        [XmlElement("EndTranslation")]
        public Vector2 EndTranslation { get; set; }
        [XmlElement("EndRotation")]
        public float EndRotation { get; set; }
    }
}
