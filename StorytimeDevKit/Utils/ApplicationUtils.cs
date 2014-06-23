using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Models.SavedData;

namespace StoryTimeDevKit.Utils
{
    public static class ApplicationUtils
    {
        private const string SavedScenePathTemplate = "{0}\\{1}.{2}";

        public static void SetupApplicationFolders()
        {
            Directory.CreateDirectory(RelativePaths.Assemblies);
            Directory.CreateDirectory(RelativePaths.Scenes);
            Directory.CreateDirectory(RelativePaths.Textures);
        }

        public static void SaveScene(SavedSceneModel scene)
        {
            string scenePath = GetPathOf(scene);
            XMLSerializerUtils.SerializeToXML(scene, scenePath);
        }

        public static string GetPathOf(SavedSceneModel scene)
        {
            return GetPathOfScene(scene.SceneName);
        }

        public static bool SceneFileExists(string sceneName)
        {
            return File.Exists(GetPathOfScene(sceneName));
        }

        private static string GetPathOfScene(string sceneName)
        {
            return string.Format(SavedScenePathTemplate,
                RelativePaths.Scenes, sceneName, FileExtensions.SceneSavedModel);
        }
    }
}
