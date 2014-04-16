using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace StoryTimeDevKit.Utils
{
    public static class XMLSerializerUtils
    {
        public static void SerializeToXML<TData>(TData data) where TData : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TData));
            TextWriter textWriter = new StreamWriter("GameObjectsPathConfiguration.xml");
            serializer.Serialize(textWriter, data);
            textWriter.Close();
        }

        public static TData DeserializeFromXML<TData>(string filePath) where TData : class
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(TData));
            TextReader textReader = new StreamReader(filePath);
            
            using(XmlReader xml = XmlReader.Create(textReader))
            {
                if (!deserializer.CanDeserialize(xml))
                {
                    return null;
                }
            }

            TData data = (TData)deserializer.Deserialize(textReader);
            textReader.Close();
            return data;
        }
    }
}
