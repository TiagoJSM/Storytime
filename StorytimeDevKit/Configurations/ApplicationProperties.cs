using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Configurations
{
    public static class ApplicationProperties
    {
        public static string DependencyInjectorKey { get { return "DependencyInjectorKey"; } }
        public static string ISceneViewerGameWorldArgName { get { return "world"; } }
        public static string IGameObjectsControllerArgName { get { return "nodeAddCB"; } }
        public static string IPuppeteerControllerArgName { get { return "world"; } }
    }
}
