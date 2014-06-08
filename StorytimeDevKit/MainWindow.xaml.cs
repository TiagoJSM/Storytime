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
using XNAControl;
using StoryTime;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Controls.Dialogs;

namespace StoryTimeDevKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageViewerDialog _imageViewer;
        public RelayCommand OpenImageViewer { get; private set; }

        public MainWindow()
        {
            OpenImageViewer = new RelayCommand(
                (o) =>
                {
                    _imageViewer = new ImageViewerDialog();
                    _imageViewer.Closed += ImageViewer_Closed;
                    _imageViewer.Show();
                },
                (o) =>
                {
                    return _imageViewer == null;
                }
            );

            InitializeComponent();
        }

        private void gameObjectsControl1_OnSceneDoubleClicked(SceneViewModel obj)
        {
            SceneViewControl.AddScene(obj);
        }

        private void ImageViewer_Closed(object sender, EventArgs e)
        {
            _imageViewer = null;
        }
    }
}
