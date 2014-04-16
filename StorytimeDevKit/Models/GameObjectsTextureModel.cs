using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoryTimeDevKit.Models
{
    public class GameObjectsTextureModel
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public IEnumerable<string> ParsedPath 
        {
            get
            {
                return new List<string>();
            }
        }
    }
}
