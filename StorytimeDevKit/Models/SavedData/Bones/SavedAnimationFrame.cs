using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedAnimationFrame
    {
        [XmlIgnore]
        public TimeSpan StartTime { get; set; }
        [XmlIgnore]
        public TimeSpan EndTime { get; set; }
        [XmlElement("StartTime")]
        public string XmlStartTime
        {
            get { return XmlConvert.ToString(StartTime); }
            set { StartTime = XmlConvert.ToTimeSpan(value); }
        }
        [XmlElement("EndTime")]
        public string XmlEndTime
        {
            get { return XmlConvert.ToString(EndTime); }
            set { EndTime = XmlConvert.ToTimeSpan(value); }
        }
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
