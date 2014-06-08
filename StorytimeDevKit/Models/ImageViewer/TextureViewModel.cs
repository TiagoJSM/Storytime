using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.ImageViewer
{
    public class TextureViewModel : BaseViewModel
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }

        public TextureViewModel(string name, string fullpath)
        {
            Name = name;
            FullPath = fullpath;
        }
    }
}
