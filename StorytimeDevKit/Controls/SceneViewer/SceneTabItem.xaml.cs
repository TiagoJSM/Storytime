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

namespace StoryTimeDevKit.Controls.SceneViewer
{
    /// <summary>
    /// Interaction logic for SceneTabItem.xaml
    /// </summary>
    public partial class SceneTabItem : UserControl
    {
        private Scene _scene;

        public Scene Scene 
        {
            get 
            { 
                return _scene; 
            }
            set
            {
                _scene = value;
                TabItemControl.Header = _scene.SceneName;
                //set active scene on game
            }
        }

        public SceneTabItem()
        {
            InitializeComponent();
        }

        public SceneTabItem(Scene s)
            :this()
        {
            Scene = s;
        }
    }
}
