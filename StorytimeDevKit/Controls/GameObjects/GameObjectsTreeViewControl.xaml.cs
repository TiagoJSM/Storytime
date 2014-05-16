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

namespace StoryTimeDevKit.Controls.GameObjects
{
    /// <summary>
    /// Interaction logic for GameObjectsTreeViewControl.xaml
    /// </summary>
    public partial class GameObjectsTreeViewControl : UserControl, IGameObjectsControl
    {
        private IGameObjectsController _controller;

        public GameObjectsTreeViewControl()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _controller = new GameObjectsController();
            _controller.Control = this;
            base.DataContext = _controller.LoadGameObjectsTree();
        }

    }
}
