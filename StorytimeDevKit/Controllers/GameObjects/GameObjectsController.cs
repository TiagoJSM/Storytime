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

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public class GameObjectsController : IGameObjectsController
    {
        private IGameObjectsControl _control;
        private GameObjectsPathConfiguration _goPathConfig;

        private class ActorsCategoryViewModel : GameObjectCategoryViewModel
        {
            public ActorsCategoryViewModel(IGameObjectsControl gameObjects)
                : base(gameObjects, "Actors", "/Images/GameObjectsControl/ActorTreeViewIcon.png", "Actors")
            { }
        }

        private class ScenesCategoryViewModel : GameObjectCategoryViewModel
        {
            public ICommand AddNewSceneCommand { get; private set; }
            public ICommand AddNewFolderCommand { get; private set; }
            public ICommand AddExistingSceneCommand { get; private set; }

            public ScenesCategoryViewModel(IGameObjectsControl gameObjects)
                : base(gameObjects, "Scenes", "/Images/GameObjectsControl/SceneTreeViewIcon.jpg", "Scenes")
            {
                AddNewSceneCommand =
                    new RelayCommand(
                        (obj) =>
                        {
                            CreateSceneDialog dialog = new CreateSceneDialog();
                            if (dialog.ShowDialog().Equals(true))
                            {
                                CreateSceneViewModel model = dialog.Model;
                                Children.Add(new SceneViewModel(this, gameObjects, model.SceneName, "full"));
                                IsExpanded = true;
                            }
                        });

                AddNewFolderCommand =
                    new RelayCommand(
                        (obj) =>
                        {
                            MessageBox.Show("New Folder!");
                        });

                AddExistingSceneCommand =
                    new RelayCommand(
                        (obj) =>
                        {
                            MessageBox.Show("existing Scene!");
                        });
            }
        }

        private class TexturesCategoryViewModel : GameObjectCategoryViewModel
        {
            public TexturesCategoryViewModel(IGameObjectsControl gameObjects)
                : base(gameObjects, "Textures", "/Images/GameObjectsControl/TextureTreeViewIcon.jpg", "Textures")
            { }
        }

        public GameObjectsController(IGameObjectsControl control)
        {
            _control = control;

            _goPathConfig = XMLSerializerUtils
                .DeserializeFromXML<GameObjectsPathConfiguration>(
                    RootConfigFiles.GameObjectsPathName
                );
        }

        public GameObjectsRoot LoadGameObjectsTree()
        {
            GameObjectsRoot root = new GameObjectsRoot();
            root.GameObjectsCategories = LoadGameObjectsCategories();
            return root;
        }

        private List<GameObjectCategoryViewModel> LoadGameObjectsCategories()
        {
            GameObjectCategoryViewModel actors = new ActorsCategoryViewModel(_control);
            GameObjectCategoryViewModel scenes = new ScenesCategoryViewModel(_control);
            GameObjectCategoryViewModel textures = new TexturesCategoryViewModel(_control);
            
            LoadActorsTree(actors);
            LoadScenesTree(scenes);
            LoadTexturesTree(textures);

            return new List<GameObjectCategoryViewModel>() { actors, scenes, textures };
        }

        private void LoadActorsTree(GameObjectCategoryViewModel actorsCategory)
        {
            Assembly assembly = Assembly.Load("StoryTimeFramework");
            AssemblyViewModel folder = new AssemblyViewModel(actorsCategory, _control, "StoryTimeFramework");

            Type baseType = typeof(BaseActor);
            List<ActorViewModel> actorTypes = assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .Select(t =>
                {
                    return new ActorViewModel(
                        folder,
                        _control, 
                        t.Name, 
                        assembly.GetName().Name);
                })
                .ToList();

            actorTypes.ForEach(a => folder.Children.Add(a));
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
                TextureViewModel texture = new TextureViewModel(folder, _control, fi.Name, fi.FullName);
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
                _control,
                fi.DirectoryName.Replace(fi.Directory.FullName, fi.Directory.Name), 
                fi.Directory.FullName);

            folders.Add(folder);
            return folder;
        }
    }
}
