using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Controllers.GameObjects;
using StoryTimeDevKit.Controllers.ImageViewer;

namespace StoryTimeDevKit.Bootstraps
{
    public static class NinjectBootstrap
    {
        public static void Configure(IKernel kernel)
        {
            kernel.Bind<ISceneViewerController>().To<SceneViewerController>().InSingletonScope();
            kernel.Bind<IGameObjectsController>().To<GameObjectsController>().InSingletonScope();
            kernel.Bind<IImageViewerController>().To<ImageViewerController>().InSingletonScope();
        }
    }
}
