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
using StoryTimeFramework.WorldManagement;
using StoryTime;
using System.ComponentModel;
using StoryTimeCore.WorldManagement;
using StoryTimeDevKit.Controllers.Scenes;

namespace StoryTimeDevKit.Controls.SceneViewer
{
    /// <summary>
    /// Interaction logic for SceneViewerControl.xaml
    /// </summary>
    public partial class SceneViewerControl : UserControl, ISceneViewerControl
    {
        private MyGame _game;
        private ISceneViewerController _controller;

        public SceneViewerControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            _game = new MyGame(userControl11.Handle);
            _controller = new SceneViewerController();
        }

        public void AddScene(Scene s)
        {
            SceneTabItem item = ScenesControl.SelectedItem as SceneTabItem;
            World.Singleton.AddScene(s);
            ScenesControl.Items.Add(new SceneTabItem(s));
        }
    }
}
