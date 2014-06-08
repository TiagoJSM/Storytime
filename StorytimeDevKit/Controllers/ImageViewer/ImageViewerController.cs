using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Models.ImageViewer;

namespace StoryTimeDevKit.Controllers.ImageViewer
{
    public class ImageViewerController : IImageViewerController
    {
        GameObjectsPathConfiguration _goPathConfig;

        public ImageViewerController()
        {
            _goPathConfig = XMLSerializerUtils
                .DeserializeFromXML<GameObjectsPathConfiguration>(
                    RootConfigFiles.GameObjectsPathName
                );
        }

        public List<TexturePathViewModel> LoadTexturePaths()
        {
            return 
                _goPathConfig
                .TexturesPaths
                .Select(tp => new TexturePathViewModel(tp))
                .ToList();
        }
    }
}
