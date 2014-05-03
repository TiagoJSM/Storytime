using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Configurations
{
    [XmlRoot("GameObjectsPathConfiguration")]
    public class GameObjectsPathConfiguration
    {
        [XmlElement("ScenesPaths")]
        public string[] ScenesPaths { get; set; }
        [XmlElement("TexturesPaths")]
        public string[] TexturesPaths { get; set; }
        [XmlElement("ActorsAssemblyPaths")]
        public string[] ActorsAssemblyPaths { get; set; }
    }
}
