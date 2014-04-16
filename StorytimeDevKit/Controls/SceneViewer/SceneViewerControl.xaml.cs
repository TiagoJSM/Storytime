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

namespace StoryTimeDevKit.Controls.SceneViewer
{
    /// <summary>
    /// Interaction logic for SceneViewerControl.xaml
    /// </summary>
    public partial class SceneViewerControl : UserControl, ISceneViewerControl
    {
        private MyGame _game;

        public SceneViewerControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                _game = new MyGame(userControl11.Handle);
            }
            
        }

        public void AddScene(Scene s)
        {
            ScenesControl.Items.Add(new SceneTabItem(s));
        }
    }
}
