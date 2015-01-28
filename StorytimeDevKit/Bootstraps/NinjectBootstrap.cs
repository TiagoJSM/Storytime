using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using StoryTimeDevKit.Controllers.ParticleEditor;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Controllers.GameObjects;
using StoryTimeDevKit.Controllers.ImageViewer;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Bootstraps
{
    public static class NinjectBootstrap
    {
        public static void MainWindowConfigure(IKernel kernel)
        {
            kernel.Bind<ISceneViewerController>().To<SceneViewerController>().InSingletonScope();
            kernel.Bind<IGameObjectsController>().To<GameObjectsController>().InSingletonScope();
            kernel.Bind<IImageViewerController>().To<ImageViewerController>().InSingletonScope();
            kernel.Bind<TransformModeViewModel>().To<TransformModeViewModel>().InSingletonScope();
        }

        public static void PuppeteerConfigure(IKernel kernel)
        {
            kernel.Bind<IPuppeteerController, ISkeletonViewerController, IAnimationTimeLineController>().To<PuppeteerController>().InSingletonScope();
            kernel.Bind<TransformModeViewModel>().To<TransformModeViewModel>().InSingletonScope();
            kernel.Bind<PuppeteerWorkingModesModel>().To<PuppeteerWorkingModesModel>().InSingletonScope();
        }

        public static void ParticleEditorConfigure(IKernel kernel)
        {
            kernel.Bind<IParticleEditorController, IParticleEmitterPropertyEditorController, IParticleEmittersController>().To<ParticleEditorController>().InSingletonScope();
        }
    }
}
