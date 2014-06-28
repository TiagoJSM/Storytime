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
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using System.ComponentModel;
using StoryTimeDevKit.Controllers.GameObjects;
using System.Globalization;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Commands.UICommands;
using System.Windows.Controls.Primitives;
using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Resources.GameObjectsTreeView;
using StoryTimeDevKit.Resources;

namespace StoryTimeDevKit.Controls.GameObjects
{
    /// <summary>
    /// Interaction logic for GameObjectsTreeViewControl.xaml
    /// </summary>
    public partial class GameObjectsTreeViewControl : UserControl, IGameObjectsControl
    {
        private IGameObjectsController _controller;

        Point _lastMouseDown;
        TreeViewItemViewModel draggedItem, _target;

        public event Action<TreeViewItemViewModel, IEnumerable<TreeViewItemViewModel>> OnGameObjectsAdded;
        public event Action<SceneViewModel> OnSceneDoubleClicked;

        public ICommand AddNewSceneCommand { get; private set; }
        public ICommand AddNewFolderCommand { get; private set; }
        public ICommand ImportSceneCommand { get; private set; }

        public GameObjectsTreeViewControl()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _controller = new GameObjectsController(this);
            base.DataContext = _controller.LoadGameObjectsTree();
            
            #region Initialize Commands
            AddNewSceneCommand =
                new RelayCommand(AddSceneTo);
            AddNewFolderCommand =
                new RelayCommand(
                    (obj) =>
                    {
                        MessageBox.Show("TODO add new folder");
                    });
            ImportSceneCommand =
                new RelayCommand(
                    (obj) =>
                    {
                        MessageBox.Show("TODO import scene");
                    });
            #endregion
        }

        private void treeView_MouseDown
            (object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(GameObjects);
            }
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(GameObjects);

                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        draggedItem = (TreeViewItemViewModel)GameObjects.SelectedItem;
                        if (draggedItem != null)
                        {
                            DragDropEffects finalDropEffect =
                                DragDrop.DoDragDrop(
                                    GameObjects,
                                    GameObjects.SelectedValue,
                                    DragDropEffects.Move);
                            //Checking target is not null and item is 
                            //dragging(moving)
                            /*if ((finalDropEffect == DragDropEffects.Move) &&
                    (_target != null))
                            {
                                // A Move drop was accepted
                                /*if (!draggedItem.Header.ToString().Equals
                    (_target.Header.ToString()))
                                {
                                    CopyItem(draggedItem, _target);
                                    _target = null;
                                    draggedItem = null;
                                }
                            }*/
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            //try
            //{
            //    Point currentPosition = e.GetPosition(GameObjects);

            //    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
            //       (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
            //    {
            //        // Verify that this is a valid drop and then store the drop target
            //        TreeViewItem item = GetNearestContainer
            //        (e.OriginalSource as UIElement);
            //        if (CheckDropTarget(draggedItem, item))
            //        {
            //            e.Effects = DragDropEffects.Move;
            //        }
            //        else
            //        {
            //            e.Effects = DragDropEffects.None;
            //        }
            //    }
            //    e.Handled = true;
            //}
            //catch (Exception)
            //{
            //}
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            //try
            //{
            //    e.Effects = DragDropEffects.None;
            //    e.Handled = true;

            //    // Verify that this is a valid drop and then store the drop target
            //    TreeViewItem TargetItem = GetNearestContainer
            //        (e.OriginalSource as UIElement);
            //    if (TargetItem != null && draggedItem != null)
            //    {
            //        _target = TargetItem;
            //        e.Effects = DragDropEffects.Move;
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

        private void GameObjectsCategory_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            StackPanel s = sender as StackPanel;
            if(s.Tag == null)
            {
                e.Handled = true;
                return;
            }
            bool contains = GameObjects.Resources.Contains(s.Tag);
            if(contains)
                s.ContextMenu = GameObjects.Resources[s.Tag] as System.Windows.Controls.ContextMenu;
        }

        public TreeView TreeView
        {
            get { return this.GameObjects; }
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
            if(OnGameObjectsAdded != null)
                OnGameObjectsAdded(parent, newModels);
        }

        private void GameObjects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GameObjects.SelectedItem is SceneViewModel && OnSceneDoubleClicked != null)
                OnSceneDoubleClicked(GameObjects.SelectedItem as SceneViewModel);
        }

        private void AddSceneTo(object obj)
        {
            TreeViewItemViewModel parent = obj as TreeViewItemViewModel;
            CreateSceneDialog dialog = new CreateSceneDialog();
            if (dialog.ShowDialog().Equals(false))
                return;

            CreateSceneViewModel model = dialog.Model;
            
            bool sceneFileExists;
            if (parent is FolderViewModel)
            {
                FolderViewModel folder = parent as FolderViewModel;
                sceneFileExists = _controller.SceneFileExistsInFolder(folder, model.SceneName);
            }
            else
                sceneFileExists = _controller.SceneFileExists(model.SceneName);

            if (sceneFileExists)
            {
                MessageBoxResult result = 
                    MessageBox.Show(
                        string.Format(LocalizedTexts.SceneAlreadyExists, model.SceneName),
                        GenericTexts.Confirmation, 
                        MessageBoxButton.YesNo);

                if (!(result == MessageBoxResult.Yes))
                    return;  
            }
            
            string path;
            if (parent is FolderViewModel)
                path = _controller.CreateScene(parent as FolderViewModel, model.SceneName);
            else
                path = _controller.CreateScene(model.SceneName);

            parent.Children.Add(new SceneViewModel(parent, this, model.SceneName, path));
            parent.IsExpanded = true;
        }

        private void AddOjectToCurrectPosition(IList<TreeViewItemViewModel> list, TreeViewItemViewModel model)
        {
        }
    }
}
