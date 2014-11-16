using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AssetListItemViewModel : BaseViewModel
    {
        private string _fullPath;

        public AssetListItemViewModel()
        {
        }

        public AssetListItemViewModel(string fullPath)
        {
            _fullPath = fullPath;
        }

        public string FullPath 
        {
            get
            {
                return _fullPath;
            }
            set
            {
                if (_fullPath == value) return;
                _fullPath = value;
                OnPropertyChanged("FullPath");
            }
        }

        public IRenderableAsset RenderableAsset { get; set; }
    }
}
