using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Configurations
{
    public static class ApplicationProperties
    {
        public static string DependencyInjectorKey { get { return "DependencyInjectorKey"; } }
        public static string ISceneViewerGraphicsContextArgName { get { return "graphicsContext"; } }
        public static string IGameObjectsControllerArgName { get { return "nodeAddCB"; } }
        public static string IPuppeteerControllerArgName { get { return "windowHandle"; } }
    }
}
