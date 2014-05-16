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

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public class GameObjectsController : IGameObjectsController
    {
        private IGameObjectsControl _control;
        private GameObjectsPathConfiguration _goPathConfig;

        public GameObjectsController()
        {
            _goPathConfig = XMLSerializerUtils
                .DeserializeFromXML<GameObjectsPathConfiguration>(
                    RootConfigFiles.GameObjectsPathName
                );
        }

        public IGameObjectsControl Control
        {
            set { _control = value; }
        }

        public GameObjectsRoot LoadGameObjectsTree()
        {
            GameObjectsRoot root = new GameObjectsRoot();
            root.GameObjectsCategories = LoadGameObjectsCategories();
            return root;
        }

        private List<GameObjectCategoryViewModel> LoadGameObjectsCategories()
        {
            GameObjectCategoryViewModel actors = new GameObjectCategoryViewModel("Actors", "/Images/GameObjectsControl/ActorTreeViewIcon.png");
            GameObjectCategoryViewModel scenes = new GameObjectCategoryViewModel("Scenes", "/Images/GameObjectsControl/SceneTreeViewIcon.jpg");
            GameObjectCategoryViewModel textures = new GameObjectCategoryViewModel("Textures", "/Images/GameObjectsControl/TextureTreeViewIcon.jpg");
            
            LoadActorsTree(actors);
            LoadScenesTree(scenes);
            LoadTexturesTree(textures);

            return new List<GameObjectCategoryViewModel>() { actors, scenes, textures };
        }

        private void LoadActorsTree(GameObjectCategoryViewModel actorsCategory)
        {
            Assembly assembly = Assembly.Load("StoryTimeFramework");
            AssemblyViewModel folder = new AssemblyViewModel(actorsCategory, "StoryTimeFramework");

            Type baseType = typeof(BaseActor);
            List<ActorViewModel> actorTypes = assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .Select(t =>
                {
                    return new ActorViewModel(
                        folder, 
                        t.Name, 
                        assembly.GetName().Name);
                })
                .ToList();

            actorsCategory.Children.Add(folder);
        }

        private void LoadScenesTree(GameObjectCategoryViewModel scenes)
        {

        }

        private void LoadTexturesTree(GameObjectCategoryViewModel texturesCategory)
        {
            List<FileInfo> fis = new List<FileInfo>();
            List<FolderViewModel> folders = new List<FolderViewModel>();

            foreach(string path in _goPathConfig.TexturesPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                fis.AddRange(di.GetFiles("*.jpg", SearchOption.AllDirectories));
            }

            foreach (FileInfo fi in fis)
            {
                FolderViewModel folder = GetFolderFor(texturesCategory, folders, fi);
                TextureViewModel texture = new TextureViewModel(folder, fi.Name, fi.FullName);
                folder.Children.Add(texture);
            }

            folders.ForEach(f => texturesCategory.Children.Add(f));
        }

        private FolderViewModel GetFolderFor(
            GameObjectCategoryViewModel texturesCategory, 
            List<FolderViewModel> folders, 
            FileInfo fi)
        {
            string folderName = fi.DirectoryName.Replace(fi.Directory.FullName, fi.Directory.Name);
            FolderViewModel folder =
                folders
                .SingleOrDefault(f => f.FolderFullPath == fi.Directory.FullName);
            
            if (folder != null)
                return folder;

            folder = new FolderViewModel(
                texturesCategory, 
                fi.DirectoryName.Replace(fi.Directory.FullName, fi.Directory.Name), 
                fi.Directory.FullName);

            folders.Add(folder);
            return folder;
        }
    }
}
