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
            List<GameObjectsActorModel> actors = _controller.LoadActors();
            foreach (GameObjectsActorModel actor in actors)
            {
                FolderTreeViewItem folder = GetFolderForAssembly(ActorsRoot, actor.AssemblyName);
                if (folder == null)
                {
                    folder = new FolderTreeViewItem();
                    folder.AssemblyName = actor.AssemblyName;
                    ActorsRoot.Items.Add(folder);
                }
                folder.Items.Add(new ActorTreeViewItem(actor));
            }
        }

        private FolderTreeViewItem GetFolderForAssembly(TreeViewItem treeViewItem, string assemblyName)
        {
            foreach (object obj in treeViewItem.Items)
            {
                FolderTreeViewItem folder = obj as FolderTreeViewItem;
                if (folder == null)
                    continue;

                if (folder.AssemblyName.Equals(assemblyName))
                    return folder;
            }
            return null;
        }
    }
}
