using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Models.ImageViewer;
using System.IO;
using StoryTimeFramework.Configurations;

namespace StoryTimeDevKit.Controllers.ImageViewer
{
    public class ImageViewerController : IImageViewerController
    {
        //GameObjectsPathConfiguration _goPathConfig;

        public ImageViewerController()
        {
        }

        public List<TexturePathViewModel> LoadTexturePaths()
        {
            return
                Directory
                .GetDirectories(RelativePaths.Textures)
                .Select(tp => new TexturePathViewModel(tp))
                .ToList();
        }
    }
}
