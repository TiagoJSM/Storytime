using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using System.Reflection;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Configurations;
using System.IO;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using System.Windows.Input;
using StoryTimeDevKit.Commands.UICommands;
using System.Windows;
using System.Windows.Controls;
using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public class GameObjectsController : IGameObjectsController
    {
        private INodeAddedCallback _nodeAddCB;

        private class TexturesCategoryViewModel : GameObjectCategoryViewModel
        {
            public TexturesCategoryViewModel(INodeAddedCallback nodeAddCB)
                : base(nodeAddCB, "Textures", "/Images/GameObjectsControl/TextureTreeViewIcon.jpg", "Textures")
            { }
        }

        public GameObjectsController(INodeAddedCallback nodeAddCB)
        {
            _nodeAddCB = nodeAddCB;
        }

        public GameObjectsRoot LoadGameObjectsTree()
        {
            var root = new GameObjectsRoot();
            root.GameObjectsCategories = LoadGameObjectsCategories();
            return root;
        }

        public string CreateScene(string sceneName)
        {
            var model = new SavedSceneModel()
            {
                SceneActors = new SavedSceneActor[0],
                SceneName = sceneName
            };

            ApplicationUtils.SaveScene(model);
            return ApplicationUtils.GetPathOf(model);
        }

        public string CreateScene(FolderViewModel folder, string sceneName)
        {
            var model = new SavedSceneModel()
            {
                SceneActors = new SavedSceneActor[0],
                SceneName = sceneName
            };

            ApplicationUtils.SaveSceneInFolder(model, folder.FolderFullPath);
            return ApplicationUtils.GetPathOfSceneInFolder(model, folder.FolderFullPath);
        }

        public bool SceneFileExists(string sceneName)
        {
            return ApplicationUtils.SceneFileExists(sceneName);
        }

        public bool SceneFileExistsInFolder(FolderViewModel folder, string sceneName)
        {
            return ApplicationUtils.SceneFileExistsInFolder(folder.FolderFullPath, sceneName);
        }

        private List<GameObjectCategoryViewModel> LoadGameObjectsCategories()
        {
            GameObjectCategoryViewModel actors = new ActorsCategoryViewModel(_nodeAddCB);
            GameObjectCategoryViewModel scenes = new ScenesCategoryViewModel(_nodeAddCB);
            //GameObjectCategoryViewModel textures = new TexturesCategoryViewModel(_control);
            
            LoadActorsTree(actors);
            LoadScenesTree(scenes);
            //LoadTexturesTree(textures);

            return new List<GameObjectCategoryViewModel>() { actors, scenes };
        }

        private void LoadActorsTree(GameObjectCategoryViewModel actorsCategory)
        {
            var assembly = Assembly.Load("StoryTimeFramework");
            var folder = new AssemblyViewModel(actorsCategory, _nodeAddCB, "StoryTimeFramework");

            var baseType = typeof(BaseActor);
            var actorTypes = assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .Select(t =>
                {
                    return new ActorViewModel(
                        folder,
                        _nodeAddCB, 
                        t, 
                        assembly.GetName().Name);
                })
                .ToList();

            actorTypes.ForEach(a => folder.Children.Add(a));
            actorsCategory.Children.Add(folder);
        }

        private void LoadScenesTree(GameObjectCategoryViewModel scenes)
        {
            var di = new DirectoryInfo(RelativePaths.Scenes);
            AddScenesAndFolders(scenes, di);
        }

        private void AddScenesAndFolders(TreeViewItemViewModel parent, DirectoryInfo currentDirectory)
        {
            var fis = currentDirectory.GetFilesByExtension(FilesExtensions.SceneSavedModel);
            foreach (var fi in fis)
            {
                var svm = new SceneViewModel(parent, _nodeAddCB, fi.Name, fi.FullName);
                parent.Children.Add(svm);
            }

            var childDirectories = currentDirectory.GetDirectories();
            foreach (var di in childDirectories)
            {
                var folder = new FolderViewModel(parent, _nodeAddCB, di.Name, di.FullName, "Scenes");
                parent.Children.Add(folder);
                AddScenesAndFolders(folder, di);
            }
        }

        private void LoadTexturesTree(GameObjectCategoryViewModel texturesCategory)
        {
            var fis = new List<FileInfo>();
            var folders = new List<FolderViewModel>();

            var textureDirectories =
                Directory
                .GetDirectories(RelativePaths.Textures);
            foreach (var path in textureDirectories)
            {
                var di = new DirectoryInfo(path);
                fis.AddRange(di.GetFiles("*.jpg", SearchOption.AllDirectories));
            }

            foreach (var fi in fis)
            {
                var folder = GetFolderFor(texturesCategory, folders, fi);
                var texture = new TextureViewModel(folder, _nodeAddCB, fi.Name, fi.FullName);
                folder.Children.Add(texture);
            }

            folders.ForEach(f => texturesCategory.Children.Add(f));
        }

        private FolderViewModel GetFolderFor(
            GameObjectCategoryViewModel texturesCategory, 
            List<FolderViewModel> folders, 
            FileInfo fi)
        {
            var folderName = fi.DirectoryName.Replace(fi.Directory.FullName, fi.Directory.Name);
            var folder =
                folders
                .SingleOrDefault(f => f.FolderFullPath == fi.Directory.FullName);
            
            if (folder != null)
                return folder;

            folder = new FolderViewModel(
                texturesCategory,
                _nodeAddCB,
                fi.DirectoryName.Replace(fi.Directory.FullName, fi.Directory.Name), 
                fi.Directory.FullName);

            folders.Add(folder);
            return folder;
        }

    }
}
