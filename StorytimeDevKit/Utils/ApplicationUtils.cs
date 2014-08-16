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
        private const string SavedScenePathTemplate = "{0}\\{1}";

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

        public static void SaveSceneInFolder(SavedSceneModel scene, string folderPath)
        {
            string scenePath = GetPathOfSceneInFolder(scene.SceneName, folderPath);
            XMLSerializerUtils.SerializeToXML(scene, scenePath);
        }

        public static string GetPathOf(SavedSceneModel scene)
        {
            return GetPathOfScene(scene.SceneName);
        }

        public static string GetPathOfSceneInFolder(SavedSceneModel scene, string folderPath)
        {
            return GetPathOfSceneInFolder(scene.SceneName, folderPath);
        }

        public static bool SceneFileExists(string sceneName)
        {
            return File.Exists(GetPathOfScene(sceneName));
        }

        private static string GetPathOfScene(string sceneName)
        {
            return string.Format(SavedScenePathTemplate,
                RelativePaths.Scenes, sceneName);
        }

        private static string GetPathOfSceneInFolder(string sceneName, string folderPath)
        {
            return string.Format(SavedScenePathTemplate,
                folderPath, sceneName);
        }

        public static bool SceneFileExistsInFolder(string folderPath, string sceneName)
        {
            return File.Exists(string.Concat(folderPath, "/",sceneName));
        }
    }
}
