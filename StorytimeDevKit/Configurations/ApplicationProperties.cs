using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Configurations
{
    public static class ApplicationProperties
    {
        public static string MainWindowDependencyInjectorKey { get { return "MainWindowDependencyInjectorKey"; } }
        public static string PuppeteerDependencyInjectorKey { get { return "PuppeteerDependencyInjectorKey"; } }
        public static string ParticleEditorInjectorKey { get { return "ParticleEditorInjectorKey"; } }
        public static string ISceneViewerGameWorldArgName { get { return "world"; } }
        public static string IGameObjectsControllerArgName { get { return "nodeAddCB"; } }
        public static string IPuppeteerControllerGameWorldArgName { get { return "gameWorld"; } }
    }
}
