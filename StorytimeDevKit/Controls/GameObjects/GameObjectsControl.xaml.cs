using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StoryTimeDevKit.Controls.GameObjects;
using StoryTimeDevKit.Controllers.GameObjects;
using StoryTimeDevKit.Models;

namespace StoryTimeDevKit.Controls.GameObjects
{
    /// <summary>
    /// Interaction logic for GameObjectsControl.xaml
    /// </summary>
    public partial class GameObjectsControl : UserControl, IGameObjectsControl
    {
        private IGameObjectsController _controller;
        public GameObjectsControl()
        {
            InitializeComponent();
            Loaded  += LoadedHandler;
            _controller = new GameObjectsController();
            _controller.Control = this;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            LoadActors();
            LoadTextures();
        }

        private void LoadActors()
        {
            List<GameObjectsActorModel> actors = _controller.LoadActors();
            TreeViewItem root = ActorsRoot;

            foreach (GameObjectsActorModel actor in actors)
            {
                FolderTreeViewItem folder = GetFolderFromTreeViewItem(root, actor.AssemblyName);
                if (folder == null)
                {
                    folder = new FolderTreeViewItem();
                    folder.FolderName = actor.AssemblyName;
                    root.Items.Add(folder);
                }
                folder.Items.Add(new ActorTreeViewItem(actor));
            }
        }

        private void LoadTextures()
        {
            List<GameObjectsTextureModel> textures = _controller.LoadTextures();
            TreeViewItem root = TexturesRoot;

            foreach (GameObjectsTextureModel texture in textures)
            {
                FolderTreeViewItem folder = GetFolderFromTreeViewItem(root, texture.RelativePath);
                if (folder == null)
                {
                    folder = new FolderTreeViewItem();
                    folder.FolderName = texture.RelativePath;
                    root.Items.Add(folder);
                }
                folder.Items.Add(new TextureTreeViewItem(texture));
            }
        }

        private FolderTreeViewItem GetFolderFromTreeViewItem(TreeViewItem treeViewItem, string name)
        {
            foreach (object obj in treeViewItem.Items)
            {
                FolderTreeViewItem folder = obj as FolderTreeViewItem;
                if (folder == null)
                    continue;

                if (folder.FolderName.Equals(name))
                    return folder;
            }
            return null;
        }
    }
}
