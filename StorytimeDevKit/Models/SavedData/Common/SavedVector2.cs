using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData.Common
{
    public class SavedVector2
    {
        [XmlElement("X")]
        public float X { get; set; }
        [XmlElement("Y")]
        public float Y { get; set; }

        public SavedVector2()
        {

        }

        public SavedVector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
