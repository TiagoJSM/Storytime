using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StoryTimeDevKit.Models.SavedData
{
    public class SavedSceneActor
    {
        [XmlElement("Module")]
        public string Module { get; set; }
        [XmlElement("ActorType")]
        public string ActorType { get; set; }
    }

    [XmlRoot("Scene")]
    public class SavedSceneModel
    {
        [XmlIgnore]
        public string SceneName { get; set; }
        [XmlElement("SceneActors")]
        public SavedSceneActor[] SceneActors { get; set; }
    }
}
