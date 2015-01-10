using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models;
using StoryTimeCore.Resources.Graphic;
using System.IO;
using StoryTimeDevKit.Extensions;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AssetListItemViewModel : BaseViewModel
    {
        private FileInfo _fileInfo;

        public AssetListItemViewModel(string fullPath)
        {
            _fileInfo = new FileInfo(fullPath);
        }

        public string FullPath 
        {
            get
            {
                return _fileInfo.FullName;
            }
        }

        public string Name
        {
            get
            {
                return _fileInfo.NameWithoutExtension();
            }
        }

        public IRenderableAsset RenderableAsset { get; set; }
    }
}
