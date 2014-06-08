using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoryTimeDevKit.Models.ImageViewer
{
    public class TexturePathViewModel : BaseViewModel
    {
        public string Path { get; set; }
        public string FolderName { get; set; }

        public TexturePathViewModel(string path)
        {
            Path = path;
            FolderName = new DirectoryInfo(Path).Name;
        }
    }
}
