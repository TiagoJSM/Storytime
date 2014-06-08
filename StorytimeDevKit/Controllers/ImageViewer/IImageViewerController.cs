using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.ImageViewer;
using StoryTimeDevKit.Controls.Displayers;

namespace StoryTimeDevKit.Controllers.ImageViewer
{
    public interface IImageViewerController : IController<IImageViewerControl>
    {
        List<TexturePathViewModel> LoadTexturePaths();
    }
}
