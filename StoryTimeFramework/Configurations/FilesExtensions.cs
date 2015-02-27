using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeFramework.Configurations
{
    public static class FilesExtensions
    {
        public static string SceneSavedModel { get { return ".ssm"; } }
        public static string SavedSkeleton { get { return ".skel"; } }
        public static string SavedAnimatedSkeleton { get { return ".askel"; } }
        public static string Assembly { get { return ".dll"; } }
    }
}
